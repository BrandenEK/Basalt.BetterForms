using Basalt.Framework.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Basalt.BetterForms;

/// <summary>
/// A configuration object that stores window settings
/// </summary>
public class BasaltSettings
{
    internal WindowState Window { get; set; } = new();

    internal static BasaltSettings CurrentConfig { get; set; } = new();

    internal class WindowState
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public bool IsMaximized { get; set; } = true;
    }

    /// <summary>
    /// Saves the current settings to the configuration file
    /// </summary>
    public void Save()
    {
        string path = Path.Combine(BasaltApplication.MainDirectory, "Settings.cfg");

        JsonSerializerSettings settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(this, settings);
        File.WriteAllText(path, json);
    }

    internal static void Load<TConfig>(string directory) where TConfig : BasaltSettings, new()
    {
        string path = Path.Combine(BasaltApplication.MainDirectory, "Settings.cfg");

        try
        {
            CurrentConfig = JsonConvert.DeserializeObject<TConfig>(File.ReadAllText(path))!;
        }
        catch
        {
            Logger.Error($"Failed to read config from {path}");
            CurrentConfig = new TConfig();
        }

        CurrentConfig.Save();
    }
}
