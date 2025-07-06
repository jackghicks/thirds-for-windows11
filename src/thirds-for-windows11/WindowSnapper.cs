using System.Diagnostics;

namespace WindowSnapManager;

/// <summary>
/// Handles window snapping logic for dividing screen into thirds using keystroke simulation.
/// Responsible for fetching screen information and determining snap zones, and executing snapping
/// via TrySnap method.
/// </summary>
public class WindowSnapper
{
    private readonly Screen _primaryScreen;

    public WindowSnapper()
    {
        _primaryScreen = Screen.PrimaryScreen ?? throw new InvalidOperationException("No primary screen found");
    }

    /// <summary>
    /// Checks if a point is in the bottom snap zone of the screen.
    /// </summary>
    public bool IsInBottomSnapZone(WinApi.POINT point)
    {
        var screenBounds = _primaryScreen.WorkingArea;
        var snapZoneHeight = 50; // pixels from bottom edge

        return point.y >= (screenBounds.Bottom - snapZoneHeight) &&
               point.x >= screenBounds.Left &&
               point.x <= screenBounds.Right;
    }

    /// <summary>
    /// Determines which third of the screen the point is in.
    /// </summary>
    public SnapZone GetSnapZone(WinApi.POINT point)
    {
        if (!IsInBottomSnapZone(point))
            return SnapZone.None;

        var screenBounds = _primaryScreen.WorkingArea;
        var thirdWidth = screenBounds.Width / 3;

        if (point.x < screenBounds.Left + thirdWidth)
            return SnapZone.LeftThird;
        else if (point.x < screenBounds.Left + (thirdWidth * 2))
            return SnapZone.MiddleThird;
        else
            return SnapZone.RightThird;
    }

    /// <summary>
    /// Test if the point given point is in a snap zonem and sends the appropriate keystrokes.
    /// </summary>
    public void TrySnap(IntPtr windowHandle, WinApi.POINT point)
    {
        var zone = GetSnapZone(point);

        if (zone == SnapZone.None)
        {
            return;
        }

        try
        {
            SendSnapKeystrokes((int)zone);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in SnapWindow: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends the actual keystroke sequence for snapping.
    /// Uses Win+Z to open Windows 11 Snap Assist, then sends the appropriate number sequence.
    /// This being "6" for the three-column layout, followed by the zone number (1, 2, or 3).
    /// </summary>
    private void SendSnapKeystrokes(int zoneNumber)
    {
        try
        {
            Task.Run(() =>
                {
                    Thread.Sleep(10);

                    SendKeyDown(WinApi.VK_LWIN);
                    SendKeyDown(WinApi.VK_Z);
                    SendKeyUp(WinApi.VK_Z);
                    SendKeyUp(WinApi.VK_LWIN);

                    Thread.Sleep(80);

                    SendKeyDown(WinApi.VK_6);
                    SendKeyUp(WinApi.VK_6);

                    Thread.Sleep(30);

                    byte zoneKey = zoneNumber switch
                    {
                        1 => WinApi.VK_1,
                        2 => WinApi.VK_2,
                        3 => WinApi.VK_3,
                    };

                    SendKeyDown(zoneKey);
                    SendKeyUp(zoneKey);

                    Debug.WriteLine($"Sent keystroke sequence: Win+Z, 6, {zoneNumber}");
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in SendSnapKeystrokes: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a key down event.
    /// </summary>
    private void SendKeyDown(byte virtualKey)
    {
        WinApi.keybd_event(virtualKey, 0, 0, UIntPtr.Zero);
    }

    /// <summary>
    /// Sends a key up event.
    /// </summary>
    private void SendKeyUp(byte virtualKey)
    {
        WinApi.keybd_event(virtualKey, 0, WinApi.KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    /// <summary>
    /// Gets information about the current screen configuration.
    /// </summary>
    public ScreenInfo GetScreenInfo()
    {
        var screenBounds = _primaryScreen.WorkingArea;
        var thirdWidth = screenBounds.Width / 3;

        return new ScreenInfo
        {
            FullBounds = new Rectangle(screenBounds.X, screenBounds.Y, screenBounds.Width, screenBounds.Height),
            LeftThird = new Rectangle(screenBounds.Left, screenBounds.Top, thirdWidth, screenBounds.Height),
            MiddleThird = new Rectangle(screenBounds.Left + thirdWidth, screenBounds.Top, thirdWidth, screenBounds.Height),
            RightThird = new Rectangle(screenBounds.Left + (thirdWidth * 2), screenBounds.Top, thirdWidth, screenBounds.Height)
        };
    }
}

/// <summary>
/// Represents the different snap zones.
/// </summary>
public enum SnapZone
{
    None,
    LeftThird,
    MiddleThird,
    RightThird
}

/// <summary>
/// Contains information about screen layout and snap zones.
/// </summary>
public class ScreenInfo
{
    public Rectangle FullBounds { get; set; }
    public Rectangle LeftThird { get; set; }
    public Rectangle MiddleThird { get; set; }
    public Rectangle RightThird { get; set; }
}
