using UnityEngine;
using Il2Cpp;
using IdleSlayerMods.Common.Extensions;
using HarmonyLib;

namespace MinionManaging
{
    public class MinionSender : MonoBehaviour
    {
        public static MinionSender Instance { get; private set; }
        public static AscensionManager _ascensionManager => AscensionManager.instance;

        private static bool ModEnabled => Plugin.Config.Enabled.Value;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Plugin.ModHelperInstance?.CreateSettingsToggle(MyPluginInfo.PLUGIN_NAME, Plugin.Config.Enabled,
                _ => Plugin.Config.Enabled.SaveEntry());
        }


        [HarmonyPatch(typeof(Minion), "CanBeClaimed")]
        public class MinionCanBeClaimed
        {
            [HarmonyPostfix]
            public static void OnMinionCanBeClaimedCheck(Minion __instance, bool __result)
            {
                if (__result && ModEnabled)
                {
                    __instance.ClaimQuest();
                }
            }
        }

        [HarmonyPatch(typeof(Minion), "IsStandingBy")]
        public class MinionIsStandingBy
        {
            [HarmonyPostfix]
            public static void OnMinionISStandingBuCheck(Minion __instance, bool __result)
            {
                if (__result && ModEnabled)
                {
                    if (__instance.GetCost() < SlayerPoints.pre)
                    {
                        __instance.GiveQuest();
                    }
                }
            }
        }
    }
}
