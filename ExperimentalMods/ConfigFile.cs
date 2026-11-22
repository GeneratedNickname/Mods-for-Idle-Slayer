using IdleSlayerMods.Common.Config;
using MelonLoader;
using UnityEngine;

namespace ExperimentalMods;

internal sealed class ConfigFile(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> Enabled;
    internal MelonPreferences_Entry<KeyCode> AscendToggleKey;
    internal MelonPreferences_Entry<KeyCode> AscendToggleKey2;
    internal MelonPreferences_Entry<bool> AscendShowPopup;
    internal MelonPreferences_Entry<KeyCode> WeeklyQuestsToggleKey;
    internal MelonPreferences_Entry<KeyCode> DailyQuestsToggleKey;

    // The keys of this developer sandbox are unbound by default: the old defaults (Z, X, Q, W)
    // collided with Auto Jump (Z) and Auto Ascending Heights (Q).
    protected override void SetBindings()
    {
        Enabled = Bind("Experimental Mods", "Enabled", true,
            "Master switch: turn the whole mod on or off (also in the in-game mod settings)");
        AscendToggleKey = Bind("Auto Ascend Mode", "AscendToggleKey", KeyCode.None,
            "Logs the current game state (debug helper). None = no key");
        AscendToggleKey2 = Bind("Auto Ascend Mode", "AscendToggleKey2", KeyCode.None,
            "Toggles the experimental jumper. None = no key");
        WeeklyQuestsToggleKey = Bind("Craftable Skills", "WeeklyQuestsToggleKey", KeyCode.None,
            "Toggles the Weekly Quests craftable skill. None = no key");
        DailyQuestsToggleKey = Bind("Craftable Skills", "DailyQuestsToggleKey", KeyCode.None,
            "Toggles the Daily Quests craftable skill. None = no key");
        AscendShowPopup = Bind("Auto Ascend Mode","AscendShowPopup", true,
            "Show a message popup to indicate whether auto Rage Mode has been toggled.");

    }
}