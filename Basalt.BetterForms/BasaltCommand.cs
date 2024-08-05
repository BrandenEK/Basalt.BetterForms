using Basalt.CommandParser;

namespace Basalt.BetterForms;

/// <summary>
/// A Basalt.CommandParser command with an option for debug mode
/// </summary>
public class BasaltCommand : CommandData
{
    /// <summary>
    /// Whether the application should be run in debug mode with a console window
    /// </summary>
    [BooleanArgument('d', "debug")]
    public bool DebugMode { get; set; } = false;
}
