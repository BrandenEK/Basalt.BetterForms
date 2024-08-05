using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using System.Diagnostics;
using System.Reflection;

namespace Basalt.BetterForms;

/// <summary>
/// A better Form with build-in logging, crash handling, and command parsing
/// </summary>
public class BasaltForm : Form
{
    /// <summary>
    /// The version of the application
    /// </summary>
    public Version CurrentVersion { get; }

    #region Contructors

    /// <summary>
    /// Constructor used in the designer
    /// </summary>
    internal BasaltForm()
    {
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new(0, 1, 0);
        Text = $"Basalt Application v{CurrentVersion.ToString(3)}";
    }

    /// <summary>
    /// Constructor used to initialize a BasaltForm
    /// </summary>
    /// <param name="init">Initialization method to run after the form is created</param>
    /// <param name="cmd">Command args info</param>
    /// <param name="title">Title of the application</param>
    /// <param name="directories">Directories that need to be created</param>
    public BasaltForm(Action<BasaltCommand> init, BasaltCommand cmd, string title, IEnumerable<string> directories)
    {
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new(0, 1, 0);
        Text = $"{title} v{CurrentVersion.ToString(3)}";

        InitializeDirectories(directories);
        InitializeCommand(cmd);
        InitializeLogging(Text, directories.First(), cmd);

        Logger.Info($"Opening {Text}");
        InitializeCore(init, cmd);

        Load += OnFormOpen;
        FormClosing += OnFormClose;
    }

    #endregion Contructors

    #region Events

    /// <summary>
    /// Called when the form is opened
    /// </summary>
    private void OnFormOpen(object? _, EventArgs e)
    {
        OnFormOpenPre();

        if (CrashException != null)
        {
            DisplayCrash(CrashException);
            return;
        }
        Application.ThreadException += (_, e) => DisplayCrash(e.Exception);

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
        if (CrashException != null)
        {
            Logger.Info($"Closing {Text}");
            return;
        }

        OnFormClose(e);

        if (!e.Cancel)
            Logger.Info($"Closing {Text}");
    }

    /// <summary>
    /// Called when the form is closed
    /// </summary>
    /// <param name="e">EventArgs that determine if the form should actually close</param>
    protected virtual void OnFormClose(FormClosingEventArgs e) { }

    #endregion Events

    #region Crash handling

    private static Exception? CrashException { get; set; }

    /// <summary>
    /// Displays a messagebox with the ex info before shutting down the application
    /// </summary>
    private static void DisplayCrash(Exception ex)
    {
        Logger.Fatal($"A crash has occured: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
        MessageBox.Show(ex.ToString(), "A crash has occured", MessageBoxButtons.OK);
        Application.Exit();
    }

    #endregion Crash handling

    #region Initialization

    /// <summary>
    /// Creates any directories necessary for the application
    /// </summary>
    private static void InitializeDirectories(IEnumerable<string> directories)
    {
        foreach (var dir in directories)
            Directory.CreateDirectory(dir);
    }

    /// <summary>
    /// Parses the cmd line arguments into the command data
    /// </summary>
    private static void InitializeCommand(BasaltCommand cmd)
    {
        TryWithCrashHandling(() => cmd.Process(Environment.GetCommandLineArgs()));
    }

    /// <summary>
    /// Adds the loggers
    /// </summary>
    private static void InitializeLogging(string title, string directory, BasaltCommand cmd)
    {
        bool debug = cmd.DebugMode || Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);

        Logger.AddLogger(new FileLogger(directory));
        if (debug)
            Logger.AddLogger(new ConsoleLogger(title));
    }

    /// <summary>
    /// Calls the InitCore method
    /// </summary>
    private static void InitializeCore(Action<BasaltCommand> init, BasaltCommand cmd)
    {
        TryWithCrashHandling(() => init(cmd));
    }

    private static void TryWithCrashHandling(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            CrashException = ex;
        }
    }

    #endregion Initialization
}
