using IdleSlayerMods.Common.Config;
using MelonLoader;
using UnityEngine;

namespace NoSpecialBoxesMode;

// Was missing from the repository. Keys, defaults and descriptions are taken from the shipped
// NoSpecialBoxesMode.dll (Live Mod Files), so existing config files keep working.
internal sealed class ConfigFile(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<KeyCode> SpecialBoxesToggleKey;
    internal MelonPreferences_Entry<bool> SpecialBoxesShowPopup;
    internal MelonPreferences_Entry<bool> NoSpecialBoxesEnabled;

    protected override void SetBindings()
    {
        SpecialBoxesToggleKey = Bind("Special Boxes Mode", "SpecialBoxesToggleKey", KeyCode.S,
            "The key bind for toggling no Special Boxes Mode");
        SpecialBoxesShowPopup = Bind("Special Boxes Mode", "SpecialBoxesShowPopup", true,
            "Show a message popup to indicate whether no Special Boxes Mode has been toggled.");
        NoSpecialBoxesEnabled = Bind("Special Boxes Mode", "NoSpecialBoxesEnabled", true,
            "Stores whether Special Boxes are enabled or disabled at game start.");
    }
}
