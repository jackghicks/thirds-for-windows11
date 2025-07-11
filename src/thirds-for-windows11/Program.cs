namespace WindowSnapManager;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var trayManager = new TrayManager();
        Application.Run();
    }    
}