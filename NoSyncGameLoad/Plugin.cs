using IdleSlayerMods.Common;
using IdleSlayerMods.Common.Extensions;
using MelonLoader;
using System.Reflection;
using MyPluginInfo = ExperimentalMods.MyPluginInfo;
using Plugin = ExperimentalMods.Plugin;

[assembly: MelonInfo(typeof(Plugin), MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION, MyPluginInfo.PLUGIN_AUTHOR)]
[assembly: MelonAdditionalDependencies("IdleSlayerMods.Common")]

namespace ExperimentalMods;

public class Plugin : MelonMod
{
    internal static ConfigFile Config;
    internal static readonly MelonLogger.Instance Logger = Melon<Plugin>.Logger;
    internal static ModHelper ModHelperInstance;

    private static bool _settingsToggleCreated;

    public override void OnInitializeMelon()
    {
        Config = new(MyPluginInfo.PLUGIN_GUID);
        Logger.Msg($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        ModHelper.ModHelperMounted += SetModHelperInstance;


    }
    private static void SetModHelperInstance(ModHelper instance) => ModHelperInstance = instance;

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Title Screen" && Config.Enabled.Value)
        {
            ModUtils.RegisterComponent<GameStart>();
        }

        if (sceneName != "Game") return;

        // The mod acts on the title screen, but the mod settings are reached in game; the switch applies from the next start.
        if (!_settingsToggleCreated && ModHelperInstance != null)
        {
            _settingsToggleCreated = true;
            ModHelperInstance.CreateSettingsToggle(MyPluginInfo.PLUGIN_NAME, Config.Enabled,
                _ => Config.Enabled.SaveEntry());
        }
    }
}
