namespace Basalt.BetterForms.Tests;

internal static class Core
{
    [STAThread]
    static void Main()
    {
        BasaltApplication.Run<TestForm, TestCommand>(InitializeCore, "Basalt Testing Form", new string[]
        {
            MainFolder
        });
    }

    static void InitializeCore(TestForm form, TestCommand cmd)
    {

    }

    public static string MainFolder { get; } = Path.Combine(Environment.CurrentDirectory, "MainFolder");
}