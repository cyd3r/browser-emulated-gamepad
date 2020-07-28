using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace BrowserGamepad.Hubs
{
    public class GamepadHub : Hub
    {
        public void AlivePing(int controllerId)
        {
            GamepadManager.MarkAsAlive(controllerId);
        }

        public void SetButton(int controllerId, string btnId, bool pressed)
        {
            Xbox360Button button;
            switch (btnId)
            {
                case "a":
                    button = Xbox360Button.A;
                    break;
                case "b":
                    button = Xbox360Button.B;
                    break;
                case "x":
                    button = Xbox360Button.X;
                    break;
                case "y":
                    button = Xbox360Button.Y;
                    break;
                case "back":
                    button = Xbox360Button.Back;
                    break;
                case "start":
                    button = Xbox360Button.Start;
                    break;
                case "rb":
                    button = Xbox360Button.RightShoulder;
                    break;
                case "lb":
                    button = Xbox360Button.LeftShoulder;
                    break;
                case "left":
                    button = Xbox360Button.Left;
                    break;
                case "right":
                    button = Xbox360Button.Right;
                    break;
                case "up":
                    button = Xbox360Button.Up;
                    break;
                case "down":
                    button = Xbox360Button.Down;
                    break;
                case "leftThumb":
                    button = Xbox360Button.LeftThumb;
                    break;
                case "rightThumb":
                    button = Xbox360Button.RightThumb;
                    break;
                case "guide":
                    button = Xbox360Button.Guide;
                    break;
                default:
                    throw new ArgumentException(btnId);
            }
            GamepadManager.SetButton(controllerId, button, pressed);
        }
        public void SetAxis(int controllerId, string axisId, short value)
        {
            Xbox360Axis axis;
            switch (axisId)
            {
                case "leftX":
                    axis = Xbox360Axis.LeftThumbX;
                    break;
                case "leftY":
                    axis = Xbox360Axis.LeftThumbY;
                    break;
                case "rightX":
                    axis = Xbox360Axis.RightThumbX;
                    break;
                case "rightY":
                    axis = Xbox360Axis.RightThumbY;
                    break;
                default:
                    throw new ArgumentException(axisId);
            }
            GamepadManager.SetAxis(controllerId, axis, value);
        }
        public void SetSlider(int controllerId, string sliderId, byte value)
        {
            Xbox360Slider slider;
            if (sliderId == "lt")
                slider = Xbox360Slider.LeftTrigger;
            else if (sliderId == "rt")
                slider = Xbox360Slider.RightTrigger;
            else
                throw new ArgumentException(sliderId);
            GamepadManager.SetSlider(controllerId, slider, value);
        }

        public async Task NewGamepad()
        {
            int controllerId = await GamepadManager.ConnectGamepad();
            await Clients.Caller.SendAsync("GamepadConnected", controllerId);
        }

        public void Disconnect(int controllerId)
        {
            GamepadManager.Disconnect(controllerId);
        }
    }
}