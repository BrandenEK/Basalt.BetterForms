using Basalt.Framework.Logging;

namespace Basalt.BetterForms;

public partial class TestForm : BasaltForm
{
    public TestForm(Action<BasaltCommand> init, TestCommand cmd, string title, IEnumerable<string> directories) : base(init, cmd, title, directories)
    {
        InitializeComponent();
    }

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
