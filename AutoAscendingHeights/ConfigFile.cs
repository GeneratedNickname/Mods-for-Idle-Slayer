using IdleSlayerMods.Common.Config;
using MelonLoader;
using UnityEngine;

namespace AutoAscendingHeights;

internal sealed class ConfigFile(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> Enabled;
    internal MelonPreferences_Entry<KeyCode> AutoAscendingHeightsToggleKey;
    internal MelonPreferences_Entry<bool> AutoAscendingHeightsShowPopup;

    protected override void SetBindings()
    {
        Enabled = Bind("Auto Ascending Heights", "Enabled", true,
            "Master switch: turn the whole mod on or off (also the toggle key and the in-game mod settings)");
        AutoAscendingHeightsToggleKey = Bind("Auto Ascending Heights", "AutoAscendingHeightsToggleKey", KeyCode.Q,
                "The key bind for toggling auto Ascending Heights");
        AutoAscendingHeightsShowPopup = Bind("Auto Ascending Heights", "AutoAscendingHeightsShowPopup", true,
                "Show a message popup to indicate whether auto Ascending Heights has been toggled.");
    }
}