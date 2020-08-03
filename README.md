# Browser Emulated Gamepad

Want to play with your friends but don't have a second gamepad lying around? Grab your phone and this piece of software to emulate a gamepad.


It is by no means a real replacement for an ordinary gamepad but if you need a gamepad real quick this should do it.

## Mappings

Not all games need all buttons and since there is not so much space on some phone screens you can choose from on of the following button sets:

| Name     | Left Stick |  A  |  B  |  X  |  Y  | LB  | RB  | RS  | LT  | RT  |
| -------- | ---------- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Unrailed | X          |  X  |  X  |     |     |     |     |     |     |     |
| Escapist | X          |  X  |  X  |  X  |  X  |  X  |  X  |  X  |  X  |  X  |

I will add more mappings as I try out more games.

## Installation

1. Install .Net: https://dotnet.microsoft.com/
2. Install ViGEmBus: https://github.com/ViGEm/ViGEmBus#installation
3. Run `dotnet restore`

## Usage

1. Run `dotnet run`
2. A new browser window will open, showing you a QR code
3. Scan the code with your phone and select a button mapping
4. Wreck your friends

Tested with dotnet version 3.1.302
