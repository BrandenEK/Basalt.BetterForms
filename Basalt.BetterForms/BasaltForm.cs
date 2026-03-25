using Basalt.Framework.Logging;

namespace Basalt.BetterForms;

/// <summary>
/// A better Form with build-in logging, crash handling, and command parsing
/// </summary>
public class BasaltForm : Form
{
    /// <summary>
    /// The version of the application
    /// </summary>
    public Version CurrentVersion { get; internal set; }

    /// <summary>
    /// Constructor used in the designer
    /// </summary>
    public BasaltForm()
    {
        CurrentVersion = new Version(0, 1, 0);
        Text = $"Basalt Application v{CurrentVersion.ToString(3)}";
    }

    /// <summary>
    /// Attaches the open and close event handlers.  Can't do this in the contructor
    /// </summary>
    internal void AttachHandlers()
    {
        Load += OnFormOpen;
        FormClosing += OnFormClose;
    }

    /// <summary>
    /// Displays a messagebox with the ex info before shutting down the application
    /// </summary>
    private static void DisplayCrash(Exception ex)
    {
        Logger.Fatal($"A crash has occured: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
        MessageBox.Show(ex.ToString(), "A crash has occured", MessageBoxButtons.OK);
        Application.Exit();
    }

    /// <summary>
    /// Checks if a point is contained in any screen's bounds
    /// </summary>
    private static bool IsPointOnScreen(Point point)
    {
        foreach (var screen in Screen.AllScreens)
        {
            var bounds = new Rectangle(screen.Bounds.Location, screen.Bounds.Size - new Size(100, 100));
            if (bounds.Contains(point))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Called when the form is opened
    /// </summary>
    private void OnFormOpen(object? _, EventArgs e)
    {
        Logger.Warn("enter");
        try
        {
            // Load window settings
            WindowSettings window = BasaltApplication.CurrentSettings.Window;
            Logger.Warn("load window");

            // Validate window settings
            if (!IsPointOnScreen(window.Location))
            {
                Logger.Error($"Window position {window.Location} is not within screen bounds");
                window.Location = new Point(0, 0);
            }
            Logger.Warn("check screen");

            // Restore window settings
            Location = window.Location;
            Size = window.Size;
            WindowState = window.IsMaximized ? FormWindowState.Maximized : FormWindowState.Normal;

            Logger.Warn("restore window");
        }
        catch (Exception ex)
        {
            BasaltApplication.CrashException = ex;
        }

        // Handle crashing
        if (BasaltApplication.CrashException != null)
        {
            DisplayCrash(BasaltApplication.CrashException);
            return;
        }
        Application.ThreadException += (_, e) => DisplayCrash(e.Exception);
        Logger.Warn("handle crash");

        // Call event
        OnFormOpen();
    }  

    /// <summary>
    /// Called when the form is closed 
    /// </summary>
    private void OnFormClose(object? _, FormClosingEventArgs e)
    {
        // Check if it is because of a crash
        if (BasaltApplication.CrashException != null)
        {
            Logger.Info($"Closing {Text}");
            return;
        }

        // Call event
        OnFormClose(e);

        // Save window settings
        BasaltApplication.CurrentSettings.Window = new WindowSettings()
        {
            Location = WindowState == FormWindowState.Normal ? Location : RestoreBounds.Location,
            Size = WindowState == FormWindowState.Normal ? Size : RestoreBounds.Size,
            IsMaximized = WindowState == FormWindowState.Maximized
        };
        BasaltSettings.Save(BasaltApplication.CurrentSettings);

        // Final message
        if (!e.Cancel)
            Logger.Info($"Closing {Text}");
    }

    /// <summary>
    /// Called when the form is opened
    /// </summary>
    protected virtual void OnFormOpen() { }

    /// <summary>
    /// Called when the form is closed
    /// </summary>
    /// <param name="e">EventArgs that determine if the form should actually close</param>
    protected virtual void OnFormClose(FormClosingEventArgs e) { }
}
