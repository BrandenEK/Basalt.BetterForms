using Basalt.CommandParser;
using Basalt.CommandParser.Attributes;

namespace Basalt.BetterForms;

/// <summary>
/// A Basalt.CommandParser command with an option for debug mode
/// </summary>
public class BasaltArguments : ProgramArguments
{
    /// <summary>
    /// Whether the application should be run in debug mode with a console window
    /// </summary>
    [BooleanArgument("debug", "d", "Runs the application in debug mode")]
    public bool DebugMode { get; set; } = false;
}
