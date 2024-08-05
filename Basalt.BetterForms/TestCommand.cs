using Basalt.CommandParser;

namespace Basalt.BetterForms;

public class TestCommand : BasaltCommand
{
    [StringArgument('t', "test")]
    public string TestArg { get; set; } = string.Empty;
}
