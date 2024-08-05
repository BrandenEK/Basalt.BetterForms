using Basalt.CommandParser;

namespace Basalt.BetterForms;

public class BasaltForm : Form
{
    public static T Initialize<T>() where T : CommandData, new()
    {
        ApplicationConfiguration.Initialize();

        return new T();
    }
}
