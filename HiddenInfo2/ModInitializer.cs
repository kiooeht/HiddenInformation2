using System.Text.Json;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Saves;
using FileAccess = Godot.FileAccess;

namespace HiddenInfo2;

[ModInitializer(nameof(InitializeMod))]
public static class ModInitializer {
    private static Config? _config;
    public static Config Config {
        get {
            _config ??= new Config();
            return _config;
        }
        private set => _config = value;
    }

    private static FileSystemWatcher _watcher = new();
    
    public static void InitializeMod() {
        new Harmony("kiooeht.HiddenInfo2").PatchAll();
        ModManager.OnModDetected += OnModDetected;
    }

    private static void OnModDetected(Mod mod) {
        if (mod.pckName != "HiddenInfo2") return;
        if (mod.assembly == null) return;

        var configPath = Path.Combine(Path.GetDirectoryName(mod.assembly.Location)!, "HiddenInfo2_config.json");
        Log.Info(configPath);
        ReadConfig(configPath);

        _watcher.Path = Path.GetDirectoryName(configPath)!;
        _watcher.Filter = Path.GetFileName(configPath);
        _watcher.NotifyFilter = NotifyFilters.LastWrite;
        _watcher.Changed += (sender, args) => {
            if (args.ChangeType == WatcherChangeTypes.Changed) {
                ReadConfig(args.FullPath);
            }
        };
        _watcher.EnableRaisingEvents = true;

        ModManager.OnModDetected -= OnModDetected;
    }

    private static void ReadConfig(string configPath) {
        using var json = new FileAccessStream(configPath, FileAccess.ModeFlags.Read);
        try {
            var config = JsonSerializer.Deserialize<Config>(json);
            if (config == null) {
                Log.Error("Hidden Info 2 config was null");
                return;
            }

            Config = config;
        }
        catch (Exception e) {
            Log.Error($"Failed to deserialize Hidden Info 2 config: {e.Message}");
        }
    }
}
