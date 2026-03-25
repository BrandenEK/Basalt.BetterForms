namespace Basalt.BetterForms.Tests;

internal static class Core
{
    [STAThread]
    static void Main()
    {
        BasaltApplication.Run<TestForm, TestArguments, TestSettings>(InitializeCore, "Basalt Testing Form", new string[]
        {
            MainFolder
        });
    }

    static void InitializeCore(TestForm form, TestArguments args, TestSettings settings)
    {
    }

    public static string MainFolder { get; } = Path.Combine(Environment.CurrentDirectory, "MainFolder");
}