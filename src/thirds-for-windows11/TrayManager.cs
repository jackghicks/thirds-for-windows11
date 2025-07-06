using System.Diagnostics;

namespace WindowSnapManager;

/// <summary>
/// Manages the system tray icon and application lifecycle.
/// </summary>
public class TrayManager : IDisposable
{
    private readonly WindowManager _windowManager;
    private readonly NotifyIcon _notifyIcon;
    private bool _disposed = false;

    public TrayManager()
    {
        _windowManager = new WindowManager();
        _notifyIcon = SetupNotifyIcon();
    }

    private NotifyIcon SetupNotifyIcon()
    {
        var notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Text = "Thirds for Windows 11",
            Visible = true
        };

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
        
        notifyIcon.ContextMenuStrip = contextMenu;
        
        return notifyIcon;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            _notifyIcon?.Dispose();
        }
    }
}
