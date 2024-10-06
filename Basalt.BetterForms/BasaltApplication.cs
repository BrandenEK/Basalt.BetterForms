using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using System.Diagnostics;
using System.Reflection;

namespace Basalt.BetterForms;

/// <summary>
/// Handles initializing and running basalt applications
/// </summary>
public static class BasaltApplication
{
    internal static Exception? CrashException { get; set; }

    internal static BasaltSettings CurrentSettings { get; set; } = new();
    internal static string MainDirectory { get; set; } = Environment.CurrentDirectory;

    /// <summary>
    /// Begins running a basalt application on the main thread
    /// </summary>
    /// <typeparam name="TForm">The type of form to create</typeparam>
    /// <typeparam name="TCommand">The type of command to create</typeparam>
    /// <typeparam name="TSettings">The type of settings to create</typeparam>
    /// <param name="init">Initialization method to run after the form is created</param>
    /// <param name="title">Title of the application</param>
    /// <param name="directories">Directories that need to be created</param>
    public static void Run<TForm, TCommand, TSettings>(Action<TForm, TCommand, TSettings> init, string title, IEnumerable<string> directories) where TForm : BasaltForm, new() where TCommand : BasaltCommand, new() where TSettings : BasaltSettings, new()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        TForm form = new();
        TCommand cmd = new();
        TSettings settings = new();

        MainDirectory = directories.First();
        form.CurrentVersion = cmd.GetType().Assembly.GetName().Version ?? new(0, 1, 0);
        form.Text = $"{title} v{form.CurrentVersion.ToString(3)}";

        InitializeDirectories(directories);
        InitializeCommand(cmd);
        InitializeLogging(form.Text, MainDirectory, cmd);
        InitializeUI(form);

        CurrentSettings = settings = BasaltSettings.Load<TSettings>();

        Logger.Info($"Opening {form.Text}");
        InitializeCore(init, form, cmd, settings);

        Application.Run(form);
    }

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
        bool debug = cmd.DebugMode || cmd.GetType().Assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);

        Logger.AddLogger(new FileLogger(directory));
        if (debug)
            Logger.AddLogger(new ConsoleLogger(title));
    }

    /// <summary>
    /// Uses reflection to call the 'InitializeComponent method on the form
    /// </summary>
    private static void InitializeUI(BasaltForm form)
    {
        MethodInfo? method = form.GetType().GetMethod("InitializeComponent", BindingFlags.NonPublic | BindingFlags.Instance);
        method?.Invoke(form, Array.Empty<object>());

        form.AttachHandlers();
    }

    /// <summary>
    /// Calls the InitCore method
    /// </summary>
    private static void InitializeCore<TForm, TCommand, TSettings>(Action<TForm, TCommand, TSettings> init, TForm form, TCommand cmd, TSettings settings)
    {
        TryWithCrashHandling(() => init(form, cmd, settings));
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
}
