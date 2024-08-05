# Better Windows Forms

Implements logging, crash handling, and command parsing in a WinForms application

### Program.cs

```cs
using Basalt.BetterForms;
using Basalt.Framework.Logging;

namespace ExampleApplication;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        BasaltApplication.Run<TestForm, TestCommand>(InitializeCore, "Test Application", new string[]
        {
            ApplicationFolder, ApplicationSubFolder
        });
    }

    static void InitializeCore(TestForm form, TestCommand cmd)
    {
        Logger.Info($"Test arg: {testCmd.TestArg}");
    }

    public static string ApplicationFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TestApp");
    public static string ApplicationSubFolder { get; } = Path.Combine(ApplicationFolder, "SubFolder");
}
```

### TestForm.cs
```cs
using Basalt.BetterForms;
using Basalt.Framework.Logging;

namespace ExampleApplication;

public partial class TestForm : BasaltForm
{
    protected override void OnFormOpenPre()
    {
        WindowState = FormWindowState.Normal;
        Location = new Point(0, 0);
        Size = new Size(1280, 720);
    }

    protected override void OnFormOpenPost()
    {
        Logger.Debug("Form is successfully open and running code");
    }
}
```

### TestCommand.cs
```cs
using Basalt.BetterForms;
using Basalt.CommandParser;

namespace ExampleApplication;

public class TestCommand : BasaltCommand
{
    [StringArgument('t', "test")]
    public string TestArg { get; set; } = string.Empty;
}
```
