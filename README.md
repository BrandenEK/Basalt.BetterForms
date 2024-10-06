# Better Windows Forms

Implements logging, configuration, crash handling, and command parsing in a WinForms application

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
        BasaltApplication.Run<TestForm, TestCommand, TestSettings>(InitializeCore, "Test Application", new string[]
        {
            ApplicationFolder, ApplicationSubFolder
        });
    }

    static void InitializeCore(TestForm form, TestCommand cmd, TestSettings settings)
    {
        Logger.Info($"Test arg: {cmd.TestArg}");
        Logger.Info($"Test setting: {settings.TextSetting}");
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
    protected override void OnFormOpen()
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

### TestSettings.cs
```cs
using Basalt.BetterForms;

namespace ExampleApplication;

public class TestSettings : BasaltSettings
{
    public string TextSetting { get; set; }
    public int NumericSetting { get; set; }
}
```