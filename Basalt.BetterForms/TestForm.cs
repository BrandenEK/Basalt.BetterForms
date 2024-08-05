using Basalt.CommandParser;

namespace Basalt.BetterForms;

public partial class TestForm : BasaltForm
{
    public TestForm(CommandData cmd, string title, string directory) : base(cmd, title, directory)
    {
        InitializeComponent();
    }
}
