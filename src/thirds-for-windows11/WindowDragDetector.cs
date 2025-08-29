using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WindowSnapManager;

/// <summary>
/// Detects when a window is being dragged and dropped by the user.
/// Emits event when the window is dropped.
/// </summary>
public class WindowDragDetector : IDisposable
{
    private WinApi.LowLevelMouseProc _proc;
    private IntPtr _hookID = IntPtr.Zero;

    // Events
    public event EventHandler<WindowDroppedEventArgs>? WindowDropped;
    public event EventHandler<WindowMoveEventArgs>? WindowMove;

    // Tracking state
    private bool _isDragging = false;
    private IntPtr _draggedWindow = IntPtr.Zero;

    public WindowDragDetector()
    {
        _proc = HookCallback;
        _hookID = SetHook(_proc);
    }

    public bool IsDragging => _isDragging;
    public IntPtr DraggedWindow => _draggedWindow;

    private IntPtr SetHook(WinApi.LowLevelMouseProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule? curModule = curProcess.MainModule)
        {
            if (curModule?.ModuleName == null)
                throw new InvalidOperationException("Unable to get main module name");

            return WinApi.SetWindowsHookEx(
                WinApi.WH_MOUSE_LL,
                proc,
                WinApi.GetModuleHandle(curModule.ModuleName),
                0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            var hookStruct = Marshal.PtrToStructure<WinApi.MSLLHOOKSTRUCT>(lParam);
            var args = new MouseHookEventArgs(hookStruct.pt, wParam);

            try
            {
                switch (wParam.ToInt32())
                {
                    case WinApi.WM_LBUTTONDOWN:
                        HandleMouseDown(args);
                        break;

                    case WinApi.WM_LBUTTONUP:
                        HandleMouseUp(args);
                        break;

                    case WinApi.WM_MOUSEMOVE:
                        HandleMouseMove(args);
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't propagate to prevent hook instability
                Debug.WriteLine($"Error in mouse hook: {ex.Message}");
            }
        }

        return WinApi.CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private void HandleMouseDown(MouseHookEventArgs args)
    {
        var windowUnderCursor = WinApi.WindowFromPoint(args.Point);

        if (windowUnderCursor != IntPtr.Zero &&
            WinApi.IsWindow(windowUnderCursor) &&
            WinApi.IsWindowVisible(windowUnderCursor))
        {
            // Check if this might be a title bar click (potential drag start)
            if (WinApi.GetWindowRect(windowUnderCursor, out var rect))
            {
                // Estimating the title bar area at about 50 pixels, are we in the top 50px?
                // TODO: Is top 50px the best we can do here?  Can we do better?
                var relativeY = args.Point.y - rect.top;
                if (relativeY >= 0 && relativeY <= 50)
                {
                    _isDragging = true;
                    _draggedWindow = windowUnderCursor;
                    Debug.WriteLine($"Started dragging window {_draggedWindow} at ({args.Point.x}, {args.Point.y})");
                }
            }
        }
    }

    private void HandleMouseUp(MouseHookEventArgs args)
    {
        if (_isDragging)
        {
            if (WinApi.IsWindow(_draggedWindow) && WinApi.IsWindowVisible(_draggedWindow))
            {
                WindowDropped?.Invoke(this, new WindowDroppedEventArgs(args.Point, _draggedWindow));
                Debug.WriteLine($"Stopped dragging window {_draggedWindow} at ({args.Point.x}, {args.Point.y})");
            }
            else
            {
                Debug.WriteLine($"Dragged window {_draggedWindow} released at ({args.Point.x}, {args.Point.y}) is no longer valid or visible.");
            }

            _isDragging = false;
            _draggedWindow = IntPtr.Zero;
        }
    }

    private void HandleMouseMove(MouseHookEventArgs args)
    {
        if (_isDragging)
        {
            WindowMove?.Invoke(this, new WindowMoveEventArgs(args.Point, _draggedWindow));
        }
    }

    public void Dispose()
    {
        if (_hookID != IntPtr.Zero)
        {
            WinApi.UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }
    }

    ~WindowDragDetector()
    {
        Dispose();
    }

    class MouseHookEventArgs : EventArgs
    {
        public WinApi.POINT Point { get; }
        public IntPtr Message { get; }

        public MouseHookEventArgs(WinApi.POINT point, IntPtr message)
        {
            Point = point;
            Message = message;
        }
    }

}

public class WindowDroppedEventArgs : EventArgs
{
    public WinApi.POINT Point { get; }
    public IntPtr WindowHandle { get; }

    public WindowDroppedEventArgs(WinApi.POINT point, IntPtr windowHandle)
    {
        Point = point;
        WindowHandle = windowHandle;
    }
}

public class WindowMoveEventArgs : EventArgs
{
    public WinApi.POINT Point { get; }
    public IntPtr WindowHandle { get; }

    public WindowMoveEventArgs(WinApi.POINT point, IntPtr windowHandle)
    {
        Point = point;
        WindowHandle = windowHandle;
    }
}