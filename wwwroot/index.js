if (location.search === "?qr") {
    const typeNumber = 4
    const errorCorrectionLevel = "L"
    const qr = qrcode(typeNumber, errorCorrectionLevel)
    qr.addData(location.origin)
    qr.make()
    document.querySelector("#qr").innerHTML = qr.createSvgTag()

    document.querySelector("#qr-entry").style.display = "unset"
    document.querySelector("#mappings").style.display = "none"
}
