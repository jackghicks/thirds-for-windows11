using System.Diagnostics;

namespace WindowSnapManager;

/// <summary>
/// Bolts the window snapper and window drag detector together with the snap zone overlay display.
/// </summary>
public class WindowManager
{
    private readonly WindowDragDetector _windowDragDetector;
    private readonly WindowSnapper _windowSnapper;
    private readonly SnapZoneOverlay _snapZoneOverlay;

    public WindowManager()
    {
        _windowDragDetector = new WindowDragDetector();
        _windowSnapper = new WindowSnapper();
        _snapZoneOverlay = new SnapZoneOverlay(_windowSnapper);
        _windowDragDetector.WindowDropped += OnWindowDropped;
        _windowDragDetector.WindowMove += OnWindowMove;
    }

    private void OnWindowDropped(object? sender, WindowDroppedEventArgs e)
    {
        _snapZoneOverlay.Hide();
        _windowSnapper.TrySnap(e.WindowHandle, e.Point);
    }
    private void OnWindowMove(object? sender, WindowMoveEventArgs e)
    {
        // Show visual feedback for the current snap zone
        var zone = _windowSnapper.GetSnapZone(e.Point);
        _snapZoneOverlay.ShowZone(zone);
    }
}