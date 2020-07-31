const range = 32767
const applyAxisRange = v => Math.min(Math.max(Math.round(v * 2 * range - range), -range), range)

const leftStickElement = document.getElementById("left-stick")
const rect = leftStickElement.getBoundingClientRect()
const leftStick = new Hammer(leftStickElement, {
    touchAction: "none",
})
leftStick.get("pan").set({ threshold: 1 })
leftStick.on("pan", (e) => {
    const x = (e.center.x - rect.left) / rect.width
    const y = (e.center.y - rect.top) / rect.height
    connection.invoke("SetAxis", controllerId, "leftX", applyAxisRange(x))
    connection.invoke("SetAxis", controllerId, "leftY", -applyAxisRange(y))
})

const btnA = document.getElementById("btn-a")
btnA.addEventListener("pointerdown", () => {
    btnA.className = "down"
    connection.invoke("SetButton", controllerId, "a", true)
})
btnA.addEventListener("pointerup", () => {
    btnA.className = ""
    connection.invoke("SetButton", controllerId, "a", false)
})

const btnB = document.getElementById("btn-b")
btnB.addEventListener("pointerdown", () => {
    btnB.className = "down"
    connection.invoke("SetButton", controllerId, "b", true)
})
btnB.addEventListener("pointerup", () => {
    btnB.className = ""
    connection.invoke("SetButton", controllerId, "b", false)
})
