using Basalt.CommandParser;

namespace Basalt.BetterForms;

public class BasaltCommand : CommandData
{
    [BooleanArgument('d', "debug")]
    public bool DebugMode { get; set; } = false;
}
