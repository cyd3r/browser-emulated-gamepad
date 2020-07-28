using System;
using System.Timers;
using System.Threading.Tasks;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

public class GamepadManager
{
    ViGEmClient client = new ViGEmClient();

    const int aliveCheckInterval = 5000;
    const int gamepadsCount = 4;
    IXbox360Controller[] gamepads = new IXbox360Controller[gamepadsCount];
    bool[] isAlive = new bool[gamepadsCount];
    Timer[] aliveTimers = new Timer[gamepadsCount];

    static GamepadManager _singleton;
    static GamepadManager singleton
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = new GamepadManager();
            }
            return _singleton;
        }
    }

    void AliveCheck(int controllerId)
    {
        if (singleton.isAlive[controllerId])
        {
            // mark for removal
            singleton.isAlive[controllerId] = false;
        }
        else
        {
            Disconnect(controllerId);
        }
    }

    void StartAliveTimer(int controllerId)
    {
        isAlive[controllerId] = true;
        // check periodically if the alive flag is still set
        aliveTimers[controllerId] = new Timer(aliveCheckInterval);
        aliveTimers[controllerId].Elapsed += (sender, e) => AliveCheck(controllerId);
        aliveTimers[controllerId].Enabled = true;
    }

    public static async Task<int> ConnectGamepad()
    {
        var gamepad = singleton.client.CreateXbox360Controller();

        // the feedback event is called immediately after the UserIndex is assigned
        // we can use this to check when the gamepad is initialised
        var compl = new TaskCompletionSource<byte>();
        Xbox360FeedbackReceivedEventHandler tmpHandler = (sender, e) => {
            compl.SetResult(e.LedNumber);
        };
        gamepad.FeedbackReceived += tmpHandler;
        gamepad.Connect();
        await compl.Task;
        gamepad.FeedbackReceived -= tmpHandler;

        Console.WriteLine("connect {0}", gamepad.UserIndex);
        singleton.gamepads[gamepad.UserIndex] = gamepad;
        singleton.StartAliveTimer(gamepad.UserIndex);
        return gamepad.UserIndex;
    }

    public static void Disconnect(int controllerId)
    {
        Console.WriteLine("disconnect {0}", controllerId);
        singleton.gamepads[controllerId].Disconnect();
        singleton.gamepads[controllerId] = null;
        singleton.isAlive[controllerId] = false;
        // cancel the alive timer and unregister all events
        singleton.aliveTimers[controllerId].Dispose();
        singleton.aliveTimers[controllerId] = null;
    }

    public static void MarkAsAlive(int controllerId)
    {
        singleton.isAlive[controllerId] = true;
    }

    public static void SetButton(int controllerId, Xbox360Button btn, bool state)
    {
        singleton.gamepads[controllerId].SetButtonState(btn, state);
    }
    public static void SetAxis(int controllerId, Xbox360Axis axis, short value)
    {
        singleton.gamepads[controllerId].SetAxisValue(axis, value);
    }
    public static void SetSlider(int controllerId, Xbox360Slider slider, byte value)
    {
        singleton.gamepads[controllerId].SetSliderValue(slider, value);
    }
}
