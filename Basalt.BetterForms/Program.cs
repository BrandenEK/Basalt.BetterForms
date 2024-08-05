using Basalt.Framework.Logging;

namespace Basalt.BetterForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TestForm(InitHandlers, new TestCommand(), "Basalt Application", new string[]
        {
            ApplicationFolder, ApplicationSubFolder
        }));
    }

    static void InitHandlers(BasaltCommand cmd)
    {
        TestCommand testCmd = (TestCommand)cmd;

        Logger.Debug("Starting init handlers");
        Logger.Info($"Test arg: {testCmd.TestArg}");
        Logger.Debug("Finishing init handlers");
    }

    public static string ApplicationFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BasaltTest");
    public static string ApplicationSubFolder { get; } = Path.Combine(ApplicationFolder, "SubFolder");
}