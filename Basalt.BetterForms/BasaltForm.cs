using Basalt.CommandParser;

namespace Basalt.BetterForms;

public class BasaltForm : Form
{
    public static void Initialize(CommandData cmd)
    {
        ApplicationConfiguration.Initialize();

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
