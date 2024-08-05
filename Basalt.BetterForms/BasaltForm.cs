using Basalt.CommandParser;
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

    public BasaltForm(CommandData cmd, string title)
    {
        try
        {
            cmd.Process(Environment.GetCommandLineArgs());
        }
        catch (Exception ex)
        {
            CrashException = ex;
        }

        Assembly assembly = Assembly.GetExecutingAssembly();

        CurrentVersion = assembly.GetName().Version ?? new(0, 1, 0);
        Text = $"{title} v{CurrentVersion.ToString(3)}";
    }

    public static void Initialize(CommandData cmd)
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

    public static Exception? CrashException { get; private set; }
}
