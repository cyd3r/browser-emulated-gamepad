"use strict";

var controllerId = -1

var connection = new signalR.HubConnectionBuilder().withUrl("/gamepadHub").build();

connection.on("GamepadConnected", (newControllerId) => {
    controllerId = newControllerId
    console.log("new controller", newControllerId)
})

connection.start()
    .then(() => {
        connection.invoke("NewGamepad")
        setInterval(() => {
            if (controllerId !== -1) {
                connection.invoke("AlivePing", controllerId)
            }
        }, 1000)
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

window.addEventListener("unload", () => connection.invoke("Disconnect", controllerId))
