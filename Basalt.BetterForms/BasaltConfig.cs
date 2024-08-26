
namespace Basalt.BetterForms;

/// <summary>
/// A configuration object that stores window settings
/// </summary>
public class BasaltConfig
{
    internal WindowState Window { get; set; } = new();

    internal class WindowState
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public bool IsMaximized { get; set; } = true;
    }
}
