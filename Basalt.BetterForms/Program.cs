namespace Basalt.BetterForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        TestCommand cmd = BasaltForm.Initialize<TestCommand>();

        TestForm form = new();

        Application.Run(form);
    }
}