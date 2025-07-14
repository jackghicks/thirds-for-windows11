using Microsoft.Win32;
using System.Reflection;

namespace WindowSnapManager;

/// <summary>
/// Manages application startup with Windows by handling registry entries.
/// </summary>
public static class StartupManager
{
    private const string REGISTRY_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string APP_NAME = "Thirds for Windows 11";

    /// <summary>
    /// Checks if the application is set to start with Windows.
    /// </summary>
    /// <returns>True if startup is enabled, false otherwise.</returns>
    public static bool IsStartupEnabled()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, false);
            return key?.GetValue(APP_NAME) != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Enables or disables the application from starting with Windows.
    /// </summary>
    /// <param name="enabled">True to enable startup, false to disable.</param>
    public static void SetStartupEnabled(bool enabled)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, true);
            if (key == null) return;

            if (enabled)
            {
                var executablePath = GetExecutablePath();
                if (!string.IsNullOrEmpty(executablePath))
                {
                    key.SetValue(APP_NAME, executablePath);
                }
            }
            else
            {
                key.DeleteValue(APP_NAME, false);
            }
        }
        catch
        {
            MessageBox.Show(
                "Could not set up to 'Start with Windows'. Please check your permissions.",
                "Thirds for Windows 11",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    /// <summary>
    /// Gets the full path to the current executable.
    /// </summary>
    /// <returns>The executable path, or null if it cannot be determined.</returns>
    private static string? GetExecutablePath()
    {
        try
        {
            return AppContext.BaseDirectory + "\\" + Assembly.GetExecutingAssembly().GetName().Name + ".exe";
        }
        catch
        {
            return null;
        }
    }
}
