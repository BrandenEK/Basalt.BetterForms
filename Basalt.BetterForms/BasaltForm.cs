using Basalt.Framework.Logging;
using Basalt.Framework.Logging.Standard;
using System.Diagnostics;
using System.Reflection;

namespace Basalt.BetterForms;

public class BasaltForm : Form
{
    public Version CurrentVersion { get; }

    internal BasaltForm()
    {
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new(0, 1, 0);
        Text = $"Basalt Application v{CurrentVersion.ToString(3)}";
    }

    public BasaltForm(BasaltCommand cmd, string title, IEnumerable<string> directories)
    {
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new(0, 1, 0);
        Text = $"{title} v{CurrentVersion.ToString(3)}";

        InitializeDirectories(directories);
        InitializeCommand(cmd);
        InitializeLogging(Text, directories.First(), cmd);
    }

    private static void InitializeDirectories(IEnumerable<string> directories)
    {
        foreach (var dir in directories)
            Directory.CreateDirectory(dir);
    }

    private static void InitializeCommand(BasaltCommand cmd)
    {
        try
        {
            cmd.Process(Environment.GetCommandLineArgs());
        }
        catch (Exception ex)
        {
            CrashException = ex;
        }
    }

    private static void InitializeLogging(string title, string directory, BasaltCommand cmd)
    {
        bool debug = cmd.DebugMode || Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);

        Logger.AddLogger(new FileLogger(directory));
        if (debug)
            Logger.AddLogger(new ConsoleLogger(title));
    }

    public static Exception? CrashException { get; private set; }
}
