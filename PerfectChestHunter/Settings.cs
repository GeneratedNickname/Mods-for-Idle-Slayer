using MelonLoader;
using IdleSlayerMods.Common.Config;
using IdleSlayerMods.Common.Extensions;
using UnityEngine;

namespace PerfectChestHunter;

internal sealed class Settings(string configName) : BaseConfig(configName)
{
    private const string Category = "Perfect Chest Hunter";

    // Mode is stored as two booleans so it can be driven by the in-game settings toggles.
    internal MelonPreferences_Entry<bool> AutoOpenChests;
    internal MelonPreferences_Entry<bool> PerfectChestsHuntOn;
    internal MelonPreferences_Entry<KeyCode> ModeCycleKey;

    internal MelonPreferences_Entry<bool> MultiplyArmoryChests;
    internal MelonPreferences_Entry<KeyCode> ArmoryChestMultiplyToggleKey;

    internal MelonPreferences_Entry<bool> ShowOverlayWhileAutoOpening;
    internal MelonPreferences_Entry<float> OverlayFontSize;
    internal MelonPreferences_Entry<float> OverlayOffsetX;
    internal MelonPreferences_Entry<float> OverlayOffsetY;

    protected override void SetBindings()
    {
        AutoOpenChests = Bind(Category, "AutoOpenChests", true,
            "Open chests automatically. When off, the mod only shows the contents overlay (Overlay mode)");
        PerfectChestsHuntOn = Bind(Category, "PerfectChestsHuntOn", true,
            "When auto-opening: open chests Perfectly (true) or Randomly (false)");
        ModeCycleKey = Bind(Category, "ModeCycleKey", KeyCode.None,
            "Key that cycles Perfect -> Random -> Overlay (None = no key)");

        MultiplyArmoryChests = Bind(Category, "MultiplyArmoryChests", false,
            "Perfect mode: open Armory Chests right after the duplicators so they get multiplied");
        // The trailing space in the key name is from the original release; kept so existing configs still load.
        ArmoryChestMultiplyToggleKey = Bind(Category, "ArmoryChestMultiplyToggleKey ", KeyCode.C,
            "The key bind for toggling Armory Chest Maximums");

        ShowOverlayWhileAutoOpening = Bind(Category, "ShowOverlayWhileAutoOpening", false,
            "Also show the contents overlay in Perfect and Random mode (it is always shown in Overlay mode)");
        OverlayFontSize = Bind(Category, "OverlayFontSize", 0f,
            "Font size of the overlay labels (0 = same size as the game's chest text)");
        OverlayOffsetX = Bind(Category, "OverlayOffsetX", 4f,
            "Horizontal distance of the overlay label from the right edge of the chest icon");
        OverlayOffsetY = Bind(Category, "OverlayOffsetY", 0f,
            "Vertical offset of the overlay label from the middle of the chest icon");
    }

    internal ChestHuntMode Mode
    {
        get => !AutoOpenChests.Value ? ChestHuntMode.Overlay
            : PerfectChestsHuntOn.Value ? ChestHuntMode.Perfect
            : ChestHuntMode.Random;
        set
        {
            AutoOpenChests.Value = value != ChestHuntMode.Overlay;
            if (value != ChestHuntMode.Overlay)
                PerfectChestsHuntOn.Value = value == ChestHuntMode.Perfect;
            AutoOpenChests.SaveEntry();
            PerfectChestsHuntOn.SaveEntry();
        }
    }
}
