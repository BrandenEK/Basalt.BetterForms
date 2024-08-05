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

    public BasaltForm(BasaltCommand cmd, string title, string directory)
    {
        // Initialize form properties
        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new(0, 1, 0);
        Text = $"{title} v{CurrentVersion.ToString(3)}";

        InitializeCommand(cmd);
        InitializeLogging(Text, directory, cmd);
    }

    private void InitializeCommand(BasaltCommand cmd)
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

    private void InitializeLogging(string title, string directory, BasaltCommand cmd)
    {
        bool debug = cmd.DebugMode || Assembly.GetExecutingAssembly().GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(x => x.IsJITTrackingEnabled);

        Logger.AddLogger(new FileLogger(directory));
        if (debug)
            Logger.AddLogger(new ConsoleLogger(title));
    }

    public static Exception? CrashException { get; private set; }
}
