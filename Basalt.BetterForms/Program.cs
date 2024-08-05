namespace Basalt.BetterForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        TestCommand cmd = new();

        TestForm form = new(cmd, "Basalt Application", ApplicationFolder);
        Application.Run(form);
    }

    public static string ApplicationFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BasaltTest");
}