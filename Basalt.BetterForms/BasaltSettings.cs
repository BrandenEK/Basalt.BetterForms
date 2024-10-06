using Basalt.Framework.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Basalt.BetterForms;

/// <summary>
/// A configuration object that stores window settings
/// </summary>
public class BasaltSettings
{
    public WindowSettings Window { get; set; } = new();

    internal static TSettings Load<TSettings>() where TSettings : BasaltSettings, new()
    {
        string path = Path.Combine(BasaltApplication.MainDirectory, "Settings.cfg");

        TSettings settings;
        try
        {
            settings = JsonConvert.DeserializeObject<TSettings>(File.ReadAllText(path))!;
        }
        catch
        {
            Logger.Error($"Failed to read config from {path}");
            settings = new TSettings();
        }

        Save(settings);
        return settings;
    }

    internal static void Save(BasaltSettings settings)
    {
        string path = Path.Combine(BasaltApplication.MainDirectory, "Settings.cfg");

        JsonSerializerSettings jss = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(settings, jss);
        File.WriteAllText(path, json);
    }
}

public class WindowSettings
{
    public Point Location { get; set; }
    public Size Size { get; set; }
    public bool IsMaximized { get; set; } = true;
}
