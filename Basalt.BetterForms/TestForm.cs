namespace Basalt.BetterForms;

public partial class TestForm : BasaltForm
{
    public TestForm(BasaltCommand cmd, string title, IEnumerable<string> directories) : base(cmd, title, directories)
    {
        InitializeComponent();
    }
}
