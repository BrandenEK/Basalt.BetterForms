using Basalt.Framework.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Basalt.BetterForms;

/// <summary>
/// A configuration object that stores window settings
/// </summary>
public class BasaltConfig
{
    internal WindowState Window { get; set; } = new();

    internal static BasaltConfig CurrentConfig { get; set; } = new();
    private static string? _configPath;

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
        if (_configPath == null)
            return;

        JsonSerializerSettings settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(this, settings);
        File.WriteAllText(_configPath, json);
    }

    internal static void Load<TConfig>(string directory) where TConfig : BasaltConfig, new()
    {
        _configPath = Path.Combine(directory, "Settings.cfg");

        try
        {
            CurrentConfig = JsonConvert.DeserializeObject<TConfig>(File.ReadAllText(_configPath))!;
        }
        catch
        {
            Logger.Error($"Failed to read config from {_configPath}");
            CurrentConfig = new TConfig();
        }

        CurrentConfig.Save();
    }
}
