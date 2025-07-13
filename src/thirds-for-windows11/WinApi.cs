using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WindowSnapManager;

/// <summary>
/// Provides access to Windows API functions for tracking the mouse and simulating keystrokes.
/// </summary>
public static class WinApi
{
    #region Constants
    public const int WH_MOUSE_LL = 14;
    public const int WM_LBUTTONDOWN = 0x0201;
    public const int WM_LBUTTONUP = 0x0202;
    public const int WM_MOUSEMOVE = 0x0200;

    // Window style constants for processability checking
    public const uint WS_VISIBLE = 0x10000000;
    public const uint WS_POPUP = 0x80000000;
    public const uint WS_THICKFRAME = 0x00040000;
    public const uint WS_CAPTION = 0x00C00000;
    public const uint WS_MINIMIZEBOX = 0x00020000;
    public const uint WS_MAXIMIZEBOX = 0x00010000;

    // Extended window style constants
    public const uint WS_EX_TOOLWINDOW = 0x00000080;

    // GetWindowLong constants
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    #endregion

    #region Structures
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Width => right - left;
        public int Height => bottom - top;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
    #endregion

    #region Delegates
    public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    #endregion

    #region DLL Imports
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(POINT Point);

    [DllImport("user32.dll")]
    public static extern bool IsWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    // Keystroke simulation functions
    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    // Additional functions for window processability checking
    [DllImport("user32.dll")]
    public static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetShellWindow();

    [DllImport("user32.dll")]
    public static extern bool IsWindowEnabled(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetWindowText(IntPtr hWnd, [Out] char[] lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetClassName(IntPtr hWnd, [Out] char[] lpClassName, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // Constants for GetWindow function
    public const uint GW_OWNER = 4;

    #endregion

    #region Keystroke Constants
    public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
    public const int KEYEVENTF_KEYUP = 0x0002;
    public const byte VK_LWIN = 0x5B;
    public const byte VK_RWIN = 0x5C;
    public const byte VK_Z = 0x5A;
    public const byte VK_1 = 0x31;
    public const byte VK_2 = 0x32;
    public const byte VK_3 = 0x33;
    public const byte VK_5 = 0x35;
    public const byte VK_6 = 0x36;
    #endregion

    #region Window Information Methods

    /// <summary>
    /// Gets the window title text.
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <returns>The window title, or empty string if none</returns>
    public static string GetWindowTitle(IntPtr hWnd)
    {
        const int maxLength = 256;
        char[] windowText = new char[maxLength];
        int length = GetWindowText(hWnd, windowText, maxLength);
        return length > 0 ? new string(windowText, 0, length) : "";
    }

    /// <summary>
    /// Gets the window class name.
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <returns>The window class name, or empty string if unable to retrieve</returns>
    public static string GetWindowClassName(IntPtr hWnd)
    {
        const int maxLength = 256;
        char[] className = new char[maxLength];
        int length = GetClassName(hWnd, className, maxLength);
        return length > 0 ? new string(className, 0, length) : "";
    }

    /// <summary>
    /// Gets information about the process that owns the window.
    /// </summary>
    /// <param name="hWnd">Handle to the window</param>
    /// <returns>Process information including name and file path, or empty values if unable to retrieve</returns>
    public static (string processName, string fileName) GetWindowProcessInfo(IntPtr hWnd)
    {
        try
        {
            uint processId;
            GetWindowThreadProcessId(hWnd, out processId);

            if (processId == 0)
                return ("", "");

            using (var process = Process.GetProcessById((int)processId))
            {
                string processName = process.ProcessName;
                string fileName = "";

                try
                {
                    fileName = process.MainModule?.FileName ?? "";
                }
                catch
                {
                    // Some system processes may not allow access to MainModule
                    fileName = "";
                }

                return (processName, fileName);
            }
        }
        catch
        {
            return ("", "");
        }
    }

    #endregion

    #region Window Processability Methods

    /// <summary>
    /// Determines if a window is processable for snapping operations based on FancyZones logic.
    /// </summary>
    /// <param name="window">Handle to the window to check</param>
    /// <returns>True if the window can be processed for snapping, false otherwise</returns>
    public static bool IsWindowProcessable(IntPtr window)
    {
        // Check if window is minimized
        if (IsIconic(window))
        {
            Debug.WriteLine($"Window {window} rejected: Window is minimized");
            return false;
        }

        var style = GetWindowLong(window, GWL_STYLE);
        var exStyle = GetWindowLong(window, GWL_EXSTYLE);

        // Check if window is visible
        if (!HasStyle(style, WS_VISIBLE))
        {
            Debug.WriteLine($"Window {window} rejected: Window is not visible");
            return false;
        }

        // Check if it's a tool window (we don't want to snap these)
        if (HasStyle(exStyle, WS_EX_TOOLWINDOW))
        {
            Debug.WriteLine($"Window {window} rejected: Window is a tool window");
            return false;
        }

        // Check popup windows - only allow certain types
        bool isPopup = HasStyle(style, WS_POPUP);
        bool hasThickFrame = HasStyle(style, WS_THICKFRAME);
        bool hasCaption = HasStyle(style, WS_CAPTION);
        bool hasMinimizeMaximizeButtons = HasStyle(style, WS_MINIMIZEBOX) || HasStyle(style, WS_MAXIMIZEBOX);

        if (isPopup && !(hasThickFrame && (hasCaption || hasMinimizeMaximizeButtons)))
        {
            // Skip popup windows that don't have proper window features
            // This filters out menus, notifications, etc.
            Debug.WriteLine($"Window {window} rejected: Popup window without proper features (no thick frame and caption/min-max buttons)");
            return false;
        }

        // Check if it has a visible owner (child window check)
        if (HasVisibleOwner(window))
        {
            Debug.WriteLine($"Window {window} rejected: Window has a visible owner");
            return false; 
        }

        // Special case for specific system window classes
        switch (GetWindowClassName(window))
        {
            case "MSTaskSwWClass":
            case "ApplicationFrameWindow":
                Debug.WriteLine($"Window {window} rejected: Window is of a system class (ApplicationFrameWindow or MSTaskSwWClass)");
                return false;
            default:
                break;
        }

        // TODO: Can we do better?  Is it possible to detect if a window is currently draggable/moveable, too?
        if(!IsWindowEnabled(window))
        {
            Debug.WriteLine($"Window {window} rejected: Window is not enabled");
            return false;
        }

        Debug.WriteLine($"Window accepted." + DescribeWindow(window));
        return true;
    }

    /// <summary>
    /// Checks if a window style has a specific flag set.
    /// </summary>
    /// <param name="style">The window style value</param>
    /// <param name="flag">The flag to check for</param>
    /// <returns>True if the flag is set</returns>
    public static bool HasStyle(uint style, uint flag)
    {
        return (style & flag) == flag;
    }

    /// <summary>
    /// Determines if a window is a root window (not a child control).
    /// </summary>
    /// <param name="window">Handle to the window to check</param>
    /// <returns>True if it's a root window</returns>
    public static bool IsRootWindow(IntPtr window)
    {
        var parent = GetParent(window);
        var desktop = GetDesktopWindow();
        return parent == IntPtr.Zero || parent == desktop;
    }

    /// <summary>
    /// Checks if a window has a visible owner window.
    /// </summary>
    /// <param name="window">Handle to the window to check</param>
    /// <returns>True if the window has a visible owner</returns>
    public static bool HasVisibleOwner(IntPtr window)
    {
        var owner = GetWindow(window, GW_OWNER);
        return owner != IntPtr.Zero && IsWindow(owner) && IsWindowVisible(owner);
    }

    public static string DescribeWindow(IntPtr hWnd)
    {
        // Get detailed window information
        string title = GetWindowTitle(hWnd);
        string className = GetWindowClassName(hWnd);
        var (processName, fileName) = GetWindowProcessInfo(hWnd);

        // Format the log message with available information
        string logMessage = $"WindowPtr: {hWnd}";

        if (!string.IsNullOrEmpty(title))
            logMessage += $" | Title: '{title}'";

        if (!string.IsNullOrEmpty(className))
            logMessage += $" | Class: '{className}'";

        if (!string.IsNullOrEmpty(processName))
            logMessage += $" | Process: '{processName}'";

        if (!string.IsNullOrEmpty(fileName))
            logMessage += $" | File: '{fileName}'";

        return logMessage;
    }

    /// <summary>
    /// Counts the number of processable windows currently open.
    /// </summary>
    /// <returns>The count of windows that can be processed for snapping operations</returns>
    public static int CountProcessableWindows()
    {
        int count = 0;

        EnumWindows((hWnd, lParam) =>
        {
            if (IsWindowProcessable(hWnd))
            {
                count++;
            }
            return true; // Continue enumeration
        }, IntPtr.Zero);

        Debug.WriteLine($"Total processable windows: {count}");
        return count;
    }

    #endregion
}
