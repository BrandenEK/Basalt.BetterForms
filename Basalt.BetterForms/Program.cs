namespace Basalt.BetterForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        TestCommand cmd = new();

        BasaltForm.Initialize(cmd);


        TestForm form = new();
        Application.Run(form);
    }
}