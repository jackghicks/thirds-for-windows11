namespace WindowSnapManager;

/// <summary>
/// Provides visual feedback overlay for snap zones, similar to Windows Snap Assist.
/// Shows semi-transparent overlays when a window is being dragged over snap zones.
/// </summary>
public class SnapZoneOverlay : IDisposable
{
    private readonly Form _overlayForm;
    private readonly WindowSnapper _windowSnapper;
    private SnapZone _currentZone = SnapZone.None;
    // Visual styling constants
    private readonly Color _overlayColor = Color.FromKnownColor(KnownColor.WindowFrame);
    private readonly Color _borderColor = Color.FromKnownColor(KnownColor.White);
    private const int BorderWidth = 5;
    private const int Margin = 5;

    public SnapZoneOverlay(WindowSnapper windowSnapper)
    {
        _windowSnapper = windowSnapper;
        _overlayForm = CreateOverlayForm();
    }

    /// <summary>
    /// Creates and configures the overlay form that will display the snap zone visual feedback.
    /// </summary>
    private Form CreateOverlayForm()
    {
        var form = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            WindowState = FormWindowState.Normal,
            TopMost = true,
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            BackColor = Color.Magenta,
            TransparencyKey = Color.Magenta,
            AllowTransparency = true,
            Opacity = 0.2,
            Enabled = false,
            Visible = false
        };

        // Set bounds to cover the working area of the primary screen
        var workingArea = Screen.PrimaryScreen?.WorkingArea ?? Rectangle.Empty;
        form.Bounds = workingArea;

        // Make the form click-through
        form.Load += (s, e) => SetClickThrough(form);
        form.Paint += OnOverlayPaint;

        return form;
    }

    /// <summary>
    /// Makes the overlay form "click-through" so it doesn't interfere with mouse interactions.
    /// This is done by making it a layered window (and transparent while we're here).
    /// </summary>
    private void SetClickThrough(Form form)
    {
        IntPtr handle = form.Handle;
        uint extendedStyle = WinApi.GetWindowLong(handle, WinApi.GWL_EXSTYLE);
        WinApi.SetWindowLong(handle, WinApi.GWL_EXSTYLE, extendedStyle | WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT);
    }

    /// <summary>
    /// Handles painting the snap zone overlays.
    /// </summary>
    private void OnOverlayPaint(object? sender, PaintEventArgs e)
    {
        if (_currentZone == SnapZone.None)
            return;

        var screenInfo = _windowSnapper.GetScreenInfo();
        Rectangle targetRect = _currentZone switch
        {
            SnapZone.LeftThird => screenInfo.LeftThird,
            SnapZone.MiddleThird => screenInfo.MiddleThird,
            SnapZone.RightThird => screenInfo.RightThird,
            _ => Rectangle.Empty
        };

        if (targetRect.IsEmpty)
            return;

        // Convert to screen coordinates relative to the overlay form
        var workingArea = Screen.PrimaryScreen?.WorkingArea ?? Rectangle.Empty;
        targetRect = new Rectangle(
            targetRect.X - workingArea.X,
            targetRect.Y - workingArea.Y,
            targetRect.Width,
            targetRect.Height
        );

        targetRect.Inflate(-Margin, -Margin);

        // Draw the filled overlay
        using (var brush = new SolidBrush(_overlayColor))
        {
            e.Graphics.FillRectangle(brush, targetRect);
        }

        // Draw the border
        using (var pen = new Pen(_borderColor, BorderWidth))
        {
            e.Graphics.DrawRectangle(pen, targetRect);
        }

        using (var brush = new SolidBrush(Color.FromKnownColor(KnownColor.Red)))
        {
            e.Graphics.FillEllipse(brush, (int)(Random.Shared.NextDouble() * _overlayForm.Width), (int)(Random.Shared.NextDouble() * _overlayForm.Height), 150, 150);
        }
    }

    /// <summary>
    /// Shows the overlay for the specified snap zone.
    /// </summary>
    public void ShowZone(SnapZone zone)
    {
        // This method will get hit repeatedly, so ignore if it's the same zone to stop flickering
        if (_currentZone == zone)
            return;

        _currentZone = zone;

        if (zone == SnapZone.None)
        {
            Hide();
        }
        else
        {
            _overlayForm.Invalidate(); // Trigger repaint
            _overlayForm.Show();
        }
    }

    /// <summary>
    /// Hides the overlay.
    /// </summary>
    public void Hide()
    {
        _currentZone = SnapZone.None;

        // re-render the form to clear the previous pane before hiding, to prevent ghosting when the next overlay shows
        _overlayForm.Invalidate();

        // Slight delay to allow the repaint to occur before hiding, to ensure the overlay clears properly
        Task.Run(() =>
            {
                Thread.Sleep(10);
                _overlayForm.Hide();
            });
    }

    public void Dispose()
    {
        _overlayForm?.Dispose();
    }
}
