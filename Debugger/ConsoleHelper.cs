using System;
using System.Runtime.InteropServices;

public static class ConsoleHelper
{
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int handle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr handle, out uint mode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr handle, uint mode);

    public static void ClearConsole()
    {
        Console.Clear();

        IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(handle, out uint mode);
        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        SetConsoleMode(handle, mode);

        Console.Write("\x1B[3J\x1B[H");
        Console.SetCursorPosition(0, 0);
    }
}