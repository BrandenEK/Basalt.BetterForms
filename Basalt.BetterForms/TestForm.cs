namespace Basalt.BetterForms;

public partial class TestForm : BasaltForm
{
    public TestForm(Action<BasaltCommand> init, TestCommand cmd, string title, IEnumerable<string> directories) : base(init, cmd, title, directories)
    {
        InitializeComponent();
    }
}
