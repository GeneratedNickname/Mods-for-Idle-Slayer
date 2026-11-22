using IdleSlayerMods.Common.Config;
using MelonLoader;
using UnityEngine;

namespace AutoJumpMod;

internal sealed class ConfigFile(string configName) : BaseConfig(configName)
{
    internal MelonPreferences_Entry<bool> UseAutoJump;
    internal MelonPreferences_Entry<KeyCode> AutoJumpToggleKey;
    internal MelonPreferences_Entry<bool> BootsLockedByMod;
//    internal MelonPreferences_Entry<double> DelayBetweenArrows;
    protected override void SetBindings()
    {
        UseAutoJump = Bind("UseAutoJump", false,
            "Master switch: turn Auto Jump on or off (also the toggle key and the in-game mod settings)");
        AutoJumpToggleKey = Bind("Auto Jump Toggle Key", "AutoJumpToggleKey", KeyCode.Z,
            "The key bind for toggling auto jump");
        BootsLockedByMod = Bind("Auto Jump Internal", "BootsLockedByMod", false,
            "Do not change - true while Auto Jump has temporarily locked the climbing boots skill, so it can be restored after a crash or quit");
//        DelayBetweenArrows = Bind("Delay Between Arrows", 0.1,
//            "Time between shooting arrows");
    }
}
