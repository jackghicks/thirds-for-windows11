using System.Diagnostics;
using System.Reflection;

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

    private Icon LoadEmbeddedIcon()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "thirds_for_windows11.res.icons.AppList.ico";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            // Fallback to system icon if embedded resource is not found
            return SystemIcons.Application;
        }

        return new Icon(stream);
    }

    private NotifyIcon SetupNotifyIcon()
    {
        var notifyIcon = new NotifyIcon
        {
            Icon = LoadEmbeddedIcon(),
            Text = "Thirds for Windows 11",
            Visible = true
        };

        var contextMenu = new ContextMenuStrip();
        
        // Get version information
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version?.ToString(3) ?? "Unknown";

        contextMenu.Items.Add(new ToolStripMenuItem($"Thirds for Windows 11 - v{version}") { Enabled = false });
        contextMenu.Items.Add(new ToolStripMenuItem("by jackghicks") { Enabled = false });
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add("Open GitHub...", null, (s, e) =>
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/jackghicks/thirds-for-windows11",
                UseShellExecute = true
            });
        });
        var startWithWindowsItem = new ToolStripMenuItem("Start with Windows?")
        {
            CheckOnClick = true,
            Checked = StartupManager.IsStartupEnabled()
        };
        startWithWindowsItem.CheckedChanged += (s, e) =>
        {
            StartupManager.SetStartupEnabled(startWithWindowsItem.Checked);
        };
        contextMenu.Items.Add(startWithWindowsItem);
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
