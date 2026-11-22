using Il2Cpp;
using IdleSlayerMods.Common.Extensions;
using UnityEngine;

namespace HawkTuah
{
    public class HawkTuahRename : MonoBehaviour
    {
        private const string OriginalName = "Hawk Eye";
        private const string NewName = "Hawk Tuah";

        public static HawkTuahRename Instance { get; private set; }

        PlayerInventory playerInventory;
        Upgrade hawkeye;


        public void Awake()
        {
            playerInventory = PlayerInventory.instance;

            foreach (var upgrade in playerInventory.upgrades)
            {
                if (upgrade.name == "upgrade_hawk_eye")
                {
                    hawkeye = upgrade;
                    break;
                }
            }

        }

        public void Start()
        {
            Plugin.ModHelperInstance?.CreateSettingsToggle(MyPluginInfo.PLUGIN_NAME, Plugin.Config.Enabled,
                _ => Plugin.Config.Enabled.SaveEntry());
        }

        public void Update()
        {
            if (hawkeye == null) return;

            // Switched off: put the original (English) name back.
            var (from, to) = Plugin.Config.Enabled.Value ? (OriginalName, NewName) : (NewName, OriginalName);
            if (hawkeye.localizedName == from)
                hawkeye.localizedName = to;
        }
    }
}
