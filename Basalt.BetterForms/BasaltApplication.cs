﻿using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace Basalt.BetterForms;

/// <summary>
/// Handles initializing and running basalt applications
/// </summary>
public static class BasaltApplication
{
    internal static Exception? CrashException { get; set; }

    /// <summary>
    /// Begins running a basalt application on the main thread
    /// </summary>
    /// <typeparam name="TForm">The type of form to create</typeparam>
    /// <typeparam name="TCommand">The type of command to create</typeparam>
    /// <typeparam name="TConfig">The type of config to create</typeparam>
    /// <param name="init">Initialization method to run after the form is created</param>
    /// <param name="title">Title of the application</param>
    /// <param name="directories">Directories that need to be created</param>
    public static void Run<TForm, TCommand, TConfig>(Action<TForm, TCommand> init, string title, IEnumerable<string> directories) where TForm : BasaltForm, new() where TCommand : BasaltCommand, new() where TConfig : BasaltConfig, new()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        TForm form = new();
        TCommand cmd = new();

        form.CurrentVersion = cmd.GetType().Assembly.GetName().Version ?? new(0, 1, 0);
        form.Text = $"{title} v{form.CurrentVersion.ToString(3)}";

        InitializeDirectories(directories);
        InitializeCommand(cmd);
        InitializeLogging(form.Text, directories.First(), cmd);
        InitializeConfig<TConfig>(directories.First());
        InitializeUI(form);

        Logger.Info($"Opening {form.Text}");
        InitializeCore(init, form, cmd);

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
    /// Parses the configuration file
    /// </summary>
    private static void InitializeConfig<TConfig>(string directory) where TConfig : BasaltConfig, new()
    {
        BasaltConfig.Load<TConfig>(directory);
    }

    /// <summary>
    /// Uses reflection to call the 'InitializeComponent method on the form
    /// </summary>
    private static void InitializeUI(BasaltForm form)
    {
        MethodInfo? method = form.GetType().GetMethod("InitializeComponent", BindingFlags.NonPublic | BindingFlags.Instance);
        method?.Invoke(form, Array.Empty<object>());
    }

    /// <summary>
    /// Calls the InitCore method
    /// </summary>
    private static void InitializeCore<TForm, TCommand>(Action<TForm, TCommand> init, TForm form, TCommand cmd)
    {
        TryWithCrashHandling(() => init(form, cmd));
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
