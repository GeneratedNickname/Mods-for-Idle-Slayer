using Il2Cpp;
using UnityEngine;
using IdleSlayerMods.Common.Extensions;

namespace BetterUpgrades;

public class DisableUpgrades: MonoBehaviour
{
    public static DisableUpgrades Instance { get; private set; }

    private PlayerInventory _playerInventory;

    private readonly String RandomBoxMagnetName = "upgrade_random_box_vertical_magnet";
    private readonly String SpecialRandomBoxMagnetName = "upgrade_special_random_box_vertical_magnet";
    private readonly String DeltaWormsName = "enemy_delta_worm_p";

    private static Boolean DisableRandomBoxMagnet => !Plugin.Settings.RandomBoxMagnet.Value;
    private static Boolean DisableSpecialRandomBoxMagnet => !Plugin.Settings.SpecialRandomBoxMagnet.Value;
    private static Boolean DisableDeltaWorms => !Plugin.Settings.DeltaWorms.Value;

    public void Awake()
    {
        Instance = this;

        _playerInventory = PlayerInventory.instance;

    }

    public void Start()
    {
        if (_playerInventory != null)
        {
            DisableUpgradesOnStart(RandomBoxMagnetName, DisableRandomBoxMagnet);

            DisableUpgradesOnStart(SpecialRandomBoxMagnetName, DisableSpecialRandomBoxMagnet);

            DisableUpgradesOnStart(DeltaWormsName, DisableDeltaWorms);
        }
    }
#if DEBUG

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LogUpgradeNamesAndDescriptions();        
        }
    }

        public void LogUpgradeNamesAndDescriptions()
        {
            var upgradesList = _playerInventory.upgrades;
            if (upgradesList == null)
            {
                Plugin.Logger.Msg("Upgrades list is null");
                return;
            }

            for (int i = 0; i < upgradesList.Length; i++)
            {
                Upgrade upgrade = upgradesList[i];
                if (upgrade == null) continue;

                Plugin.Logger.Msg($"Upgrade [{i}]: Name: {upgrade.name}, LocalName: {upgrade.localizedName}, Description: {upgrade.localizedDescription}");
            }
        }
    
#endif 

    public void DisableUpgradesOnStart(String upgradeName, Boolean PluginSetting)
    {
        var upgradesList = _playerInventory.upgrades;
        if (upgradesList == null)
        {
            Plugin.Logger.Debug("Upgrades list is null");
            return;
        }

        Upgrade upgradeToDisable = null;

        foreach (Upgrade upgrade in upgradesList)
        {
            if (upgrade == null) continue;
            if (upgrade.name == upgradeName)
            {
                upgradeToDisable = upgrade;
                break;
            }
        }

        if (upgradeToDisable == null) return;

        var disabledByMod = DisabledByModNames();

        if (PluginSetting)
        {
            upgradeToDisable.disabled = true;
            upgradeToDisable.bought = false;
            disabledByMod.Add(upgradeName);

            Plugin.Logger.Msg($"Upgrade {upgradeToDisable.localizedName} disabled");
        }
        else if (disabledByMod.Remove(upgradeName) && upgradeToDisable.disabled)
        {
            // Setting was switched back on: undo our own disable. Only upgrades this mod disabled are touched.
            // The upgrade was un-bought when it was disabled, so it has to be bought again.
            upgradeToDisable.disabled = false;
            Plugin.Logger.Msg($"Upgrade {upgradeToDisable.localizedName} re-enabled");
        }

        SaveDisabledByModNames(disabledByMod);
    }

    private static HashSet<string> DisabledByModNames() =>
        new(Plugin.Settings.DisabledByMod.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

    private static void SaveDisabledByModNames(HashSet<string> names)
    {
        var value = string.Join(",", names.OrderBy(n => n));
        if (value == Plugin.Settings.DisabledByMod.Value) return;
        Plugin.Settings.DisabledByMod.Value = value;
        Plugin.Settings.DisabledByMod.SaveEntry();
    }

/*
    [HarmonyPatch(typeof(ShopManager), "OpenUpgrades")]
    public class UpgradePanelOpened
    {
        [HarmonyPostfix]
        static void UpgradesVisible()
        {
            //DisableUpgrades.Instance?.TriggerUpgradeDisable();
        }
    }

    [HarmonyPatch(typeof(ShopManager), "BuyAllUpgrades")]
    public class AllUpgradesPurchased
    {
        [HarmonyPostfix]
        static void UpgradesPurchased()
        {
            //DisableUpgrades.Instance?.TriggerUpgradeDisable();
        }
    }*/
}