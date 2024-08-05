namespace Basalt.BetterForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        TestCommand cmd = new();

        //BasaltForm.Initialize(cmd);


        TestForm form = new(cmd, "Basalt Application");
        Application.Run(form);
    }
}