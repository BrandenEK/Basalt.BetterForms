namespace Basalt.BetterForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        TestCommand cmd = new();

        TestForm form = new(cmd, "Basalt Application", new string[]
        {
            ApplicationFolder, ApplicationSubFolder
        });
        Application.Run(form);
    }

    public static string ApplicationFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BasaltTest");
    public static string ApplicationSubFolder { get; } = Path.Combine(ApplicationFolder, "SubFolder");
}