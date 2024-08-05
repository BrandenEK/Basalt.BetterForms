using Basalt.CommandParser;

namespace Basalt.BetterForms;

public partial class TestForm : BasaltForm
{
    public TestForm(CommandData cmd, string title) : base(cmd, title)
    {
        InitializeComponent();
    }
}
