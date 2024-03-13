using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.InteropServices;

public class KeyPressed
{
    public bool IsKeyPressed { get; set; }
}

class Program
{
    // Import necessary functions from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    // Define the low-level keyboard hook code
    private const int WH_KEYBOARD_LL = 13;

    // Define the delegate for the hook procedure
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // Store the hook handle for later unhooking
    private static IntPtr hookHandle = IntPtr.Zero;

    static SerialPort _serialPort;

    static KeyPressed _keyPressed;
    static void Main()
    {
        // Set up the low-level keyboard hook
        hookHandle = SetHook(HookCallback);

        _keyPressed = new KeyPressed { IsKeyPressed = false };

        _serialPort = new SerialPort();
        _serialPort.PortName = "COM9";//Set your board COM
        _serialPort.BaudRate = 9600;
        _serialPort.Open();

        Application.Run();

        // Unhook the low-level keyboard hook before exiting
        UnhookWindowsHookEx(hookHandle);

        _serialPort.Close();
    }

    // Set the low-level keyboard hook
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process currentProcess = Process.GetCurrentProcess())
        using (ProcessModule currentModule = currentProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(currentModule.ModuleName), 0);
        }
    }

    // The callback function for the low-level keyboard hook
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // Check if the key event should be processed
        if (!_keyPressed.IsKeyPressed && (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)))
        {
            // Extract the key information from the lParam
            KBDLLHOOKSTRUCT keyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            if ($"{(Keys)keyInfo.vkCode}".Equals("Oem2"))
            {
                Console.WriteLine("Pump on!");
                _keyPressed.IsKeyPressed = true;
                _serialPort.Write("1");
            }
        }

        if (_keyPressed.IsKeyPressed && (nCode >= 0 && (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYDOWN)))
        {
            // Extract the key information from the lParam
            KBDLLHOOKSTRUCT keyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            if ($"{(Keys)keyInfo.vkCode}".Equals("Oem2"))
            {
                Console.WriteLine("Pump off!");
                _keyPressed.IsKeyPressed = false;
                _serialPort.Write("0");
            }

            
        }


        // Call the next hook in the chain
        return CallNextHookEx(hookHandle, nCode, wParam, lParam);
    }

    // Define the structure for the low-level keyboard hook information
    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    // Define constants for key events
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private const int WM_SYSKEYDOWN = 0x0104;
}