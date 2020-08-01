"use strict";

let controllerId = -1

const connection = new signalR.HubConnectionBuilder().withUrl("/gamepadHub").build();

const setupButtons = () => {
    const buttons = ["a", "b", "x", "y", "rb", "lb", "rightThumb"]
    buttons.forEach(btn => {
        const element = document.getElementById(`btn-${btn}`)
        const hammer = new Hammer(element, { touchAction: "none"})
        if (element !== null) {
            element.addEventListener("pointerdown", (e) => {
                e.preventDefault()
                connection.invoke("SetButton", controllerId, btn, true)
            })
            element.addEventListener("pointerup", (e) => {
                e.preventDefault()
                connection.invoke("SetButton", controllerId, btn, false)
            })
            element.addEventListener("touchstart", (e) => {
                e.preventDefault()
            })
        } else {
            console.log("skip", btn)
        }
    })
}
const updateStickRadius = (radius) => {
    const el = document.querySelector("#stick-left .radius")
    el.style.height = `${2 * radius}px`
    el.style.width = `${2 * radius}px`
}
const setupLeftStick = () => {
    const leftStickElement = document.getElementById("stick-left")
    const range = 32767

    const applyAxisRange = (x, y) => {
        const rect = leftStickElement.getBoundingClientRect()
        // find the radius of the control circle
        const radius = .5 * Math.min(rect.width, rect.height) - 10
        updateStickRadius(radius)
        const centre = [rect.left + .5 * rect.width, rect.top + .5 * rect.height]

        const xPos = x - centre[0]
        const yPos = y - centre[1]

        const dist = Math.max(Math.sqrt(xPos * xPos + yPos * yPos), radius)
        const xNorm = range / dist * xPos
        const yNorm = range / dist * yPos

        return [xNorm, yNorm]
    }

    const leftStick = new Hammer(leftStickElement, {
        touchAction: "none",
    })
    leftStick.get("pan").set({ threshold: 1 })
    leftStick.on("pan", (e) => {
        const [x, y] = applyAxisRange(e.center.x, e.center.y)
        console.log(x, y)
        connection.invoke("SetAxis", controllerId, "leftX", Math.round(x))
        connection.invoke("SetAxis", controllerId, "leftY", -Math.round(y))
    })
    leftStick.on("panend", (e) => {
        console.log(e)
        connection.invoke("SetAxis", controllerId, "leftX", 0)
        connection.invoke("SetAxis", controllerId, "leftY", 0)
    })
}
const setupSlider = () => {
    const buttons = ["lt", "rt"]
    buttons.forEach(btn => {
        const element = document.getElementById(`btn-${btn}`)
        new Hammer(element, { touchAction: "none" })
        if (element !== null) {
            element.addEventListener("pointerdown", () => {
                connection.invoke("SetSlider", controllerId, btn, 255)
            })
            element.addEventListener("pointerup", () => {
                connection.invoke("SetSlider", controllerId, btn, 0)
            })
        } else {
            console.log("skip", btn)
        }
    })
}

connection.on("GamepadConnected", (newControllerId) => {
    controllerId = newControllerId
    console.log("new controller", newControllerId)
    setupButtons()
    setupLeftStick()
    setupSlider()
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
