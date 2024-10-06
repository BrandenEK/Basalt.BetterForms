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
    /// Called when the form is opened
    /// </summary>
    private void OnFormOpen(object? _, EventArgs e)
    {
        OnFormOpenPre();

        // Load window settings
        BasaltSettings.WindowSettings window = BasaltApplication.CurrentSettings.Window;
        WindowState = window.IsMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
        Location = window.Location;
        Size = window.Size;

        // Handle crashing
        if (BasaltApplication.CrashException != null)
        {
            DisplayCrash(BasaltApplication.CrashException);
            return;
        }
        Application.ThreadException += (_, e) => DisplayCrash(e.Exception);

        // Call event
        OnFormOpenPost();
    }

    /// <summary>
    /// Called before the exception check when the form is opened
    /// </summary>
    protected virtual void OnFormOpenPre() { }

    /// <summary>
    /// Called after the exception check when the form is opened
    /// </summary>
    protected virtual void OnFormOpenPost() { }

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
        BasaltApplication.CurrentSettings.Window = new BasaltSettings.WindowSettings()
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
    /// Called when the form is closed
    /// </summary>
    /// <param name="e">EventArgs that determine if the form should actually close</param>
    protected virtual void OnFormClose(FormClosingEventArgs e) { }
}
