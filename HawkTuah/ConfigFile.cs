using IdleSlayerMods.Common.Config;
using MelonLoader;

namespace HawkTuah;

internal sealed class ConfigFile(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> Enabled;

    protected override void SetBindings()
    {
        Enabled = Bind("Enabled", true,
            "Master switch: turn the whole mod on or off (also in the in-game mod settings)");
    }
}
