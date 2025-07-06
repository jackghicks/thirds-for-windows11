using System.Diagnostics;

namespace WindowSnapManager;

/// <summary>
/// Bolts the window snapper and window drag detector together.
/// </summary>
public class WindowManager
{
    private readonly WindowDragDetector _windowDragDetector;
    private readonly WindowSnapper _windowSnapper;

    public WindowManager()
    {
        _windowDragDetector = new WindowDragDetector();
        _windowSnapper = new WindowSnapper();
        _windowDragDetector.WindowDropped += OnWindowDropped;
    }

    private void OnWindowDropped(object? sender, WindowDroppedEventArgs e)
    {
        _windowSnapper.TrySnap(e.WindowHandle, e.Point);
    }
}