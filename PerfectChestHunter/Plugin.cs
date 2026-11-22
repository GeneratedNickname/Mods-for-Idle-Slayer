using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using IdleSlayerMods.Common;
using MelonLoader;
using MyPluginInfo = PerfectChestHunter.MyPluginInfo;
using Plugin = PerfectChestHunter.Plugin;

[assembly: MelonInfo(typeof(Plugin), MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION, MyPluginInfo.PLUGIN_AUTHOR)]
[assembly: MelonAdditionalDependencies("IdleSlayerMods.Common")]

namespace PerfectChestHunter;

public class Plugin : MelonMod
{
    internal static Settings Settings;
    internal static ModHelper ModHelperInstance;
    internal static readonly MelonLogger.Instance Logger = Melon<Plugin>.Logger;

    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<ChestHuntController>();
        Settings = new(MyPluginInfo.PLUGIN_GUID);
        Logger.Msg($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        ModHelper.ModHelperMounted += SetModHelperInstance;
    }

    private static void SetModHelperInstance(ModHelper instance) => ModHelperInstance = instance;

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName != "Game") return;

        var chestHunt = GameObject.Find("Chest Hunt");
        if (chestHunt)
            chestHunt.AddComponent<ChestHuntController>();
    }

    /// <summary>Logs only in Debug builds; the call and its message are compiled out of Release builds.</summary>
    [System.Diagnostics.Conditional("DEBUG")]
    internal static void LogDebug(string message) => Logger.Msg(message);
}
