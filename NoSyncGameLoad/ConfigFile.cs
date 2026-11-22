using IdleSlayerMods.Common.Config;
using MelonLoader;

// This project reuses the ExperimentalMods namespace (see Plugin.cs / MyPluginInfo.cs).
namespace ExperimentalMods;

internal sealed class ConfigFile(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> Enabled;

    protected override void SetBindings()
    {
        Enabled = Bind("Enabled", true,
            "Master switch: skip the title screen and load the game directly (also in the in-game mod settings)");
    }
}
