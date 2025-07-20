# Thirds for Windows 11

![Thirds for Windows 11 Icon](src/thirds-for-windows11/res/icons/AppList.targetsize-256.png)

## Why?

Using an ultra-wide monitor, I find myself working in 3 columns across the screen.  Windows 11 has built in support for this, but it is buried in Snap Assist menus.  Unfortunately, there's no way to directly expose the thirds functionality, and so you're forced to deal with menu diving in the fiddly interface each time.

This is a simple application which detects window drag and drop behaviour and, if the user drags a window to the bottom of the screen and releases it, will trigger the necessary keystrokes to use the Windows 11 Snap Assist feature to snap the dropped window to maximise in the corresponding third of the screen.

The keystrokes approach is not perfect, but works in lieu of proper Windows API functionality.  Currently, Snap Assist doesn't seem to be available in any meaningful way through the API, and so we're forced to compromise with this approach for now.

I wanted to focus on keeping the snapping behaviour inside Windows's built in Snap Assist functionality as much as possible, because:
* Setting window position and size directly does not work consistently across different types and classes of window.
* Snap Assist has other features which are really great, such as resizing the columns and having neighboring windows resize to match.  If you set the window size and position using Windows API, that no longer happens.

## What are the keystrokes?

In general, it's this sequence:
1. `WinKey + Z` - Opens the Snap Assist menu for the currently focussed window.
2. `6` - Selects thirds (usually, but not always 6, see below)
3. `1/2/3` - Selects the specific third, and commences the snap.

Note that the 6 seems to depend on how many windows are currently open and considered "snappable".  It seems that:
* 1 Window - Thirds are on key `3`
* 2 Windows - Thirds are on key `5`
* 3 Windows or more - Thirds are on key `6`

The logic for this is inside the WindowSnapper class.

## Known Issues

* You do see a small flicker of the Snap Assist menu as it activates.  This is just inherent in the way the program is working, and an acceptable compromise (I think) to keep within the auspices of the otherwise default Snap Assist functionality.

## Contributing

I am already daily-driving this program, and will keep this repo up to date with my latest changes as I find problems and improve it.  If you find issues, please let me know in the issue tracker, and similarly if you end up fixing them yourself I'll be very grateful to receive PRs.

## Build

This is a compeltely standard .NET Winforms project:

```
dotnet clean
dotnet build
dotnet publish
```

## Rebuilding the Icon File

ImageMagick can be used to quickly bundle different size icon PNGs together into a single ico file, like this:

```
cd .\src\thirds-for-windows11\res\icons 
magick.exe *.png AppList.ico
```