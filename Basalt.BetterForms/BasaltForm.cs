﻿using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using System.Diagnostics;
using System.Reflection;

namespace Basalt.BetterForms;

public class BasaltForm : Form
{
    public Version CurrentVersion { get; }

    #region Contructors

    internal BasaltForm()
    {
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new(0, 1, 0);
        Text = $"Basalt Application v{CurrentVersion.ToString(3)}";
    }

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

    protected virtual void OnFormOpenPre() { }

    protected virtual void OnFormOpenPost() { }

    private void OnFormClose(object? _, FormClosingEventArgs e)
    {
        OnFormClose(e);
        Logger.Info($"Closing {Text}");
    }

    protected virtual void OnFormClose(FormClosingEventArgs e) { }

    #endregion Events

    #region Crash handling

    public static Exception? CrashException { get; private set; }

    private static void DisplayCrash(Exception ex)
    {
        Logger.Fatal($"A crash has occured: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
        MessageBox.Show(ex.ToString(), "A crash has occured", MessageBoxButtons.OK);
        Application.Exit();
    }

    #endregion Crash handling

    #region Initialization

    private static void InitializeDirectories(IEnumerable<string> directories)
    {
        foreach (var dir in directories)
            Directory.CreateDirectory(dir);
    }

    private static void InitializeCommand(BasaltCommand cmd)
    {
        TryWithCrashHandling(() => cmd.Process(Environment.GetCommandLineArgs()));
    }

    private static void InitializeLogging(string title, string directory, BasaltCommand cmd)
    {
        bool debug = cmd.DebugMode || Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);

        Logger.AddLogger(new FileLogger(directory));
        if (debug)
            Logger.AddLogger(new ConsoleLogger(title));
    }

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
