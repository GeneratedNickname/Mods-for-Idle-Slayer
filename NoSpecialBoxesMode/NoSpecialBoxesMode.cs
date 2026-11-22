using System.Collections;
using HarmonyLib;
using IdleSlayerMods.Common.Extensions;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace NoSpecialBoxesMode
{
    public class NoSpecialBoxes : MonoBehaviour
    {
        public static NoSpecialBoxes Instance { get; private set; }

        private bool _specialBoxesEnabled;

        private double _specialRandomBoxChanceBackup;

        private MapController _mapController;
        private PlayerInventory _playerInventory;

        private Maps _maps;

        private void Awake()
        {
            Instance = this;

            Plugin.Logger.Debug("NoSpecialBoxes Awake() called");

            _mapController = MapController.instance;
            _playerInventory = PlayerInventory.instance;

            _specialBoxesEnabled = Plugin.Config.NoSpecialBoxesEnabled.Value;
            _specialRandomBoxChanceBackup = _playerInventory.specialRandomBoxChance;

            if (_specialRandomBoxChanceBackup < 1f)
                _specialRandomBoxChanceBackup = 8f;

            ToggleSpecialBoxes();

            _maps = Maps.list;
            
            if (_maps == null)
            {
                Plugin.Logger.Debug("Maps instance not initialized. Check your scene setup.");
            }
        }

        private static bool ModEnabled => Plugin.Config.Enabled.Value;

        private void Start()
        {
            Plugin.ModHelperInstance?.CreateSettingsToggle(MyPluginInfo.PLUGIN_NAME, Plugin.Config.Enabled,
                _ =>
                {
                    Plugin.Config.Enabled.SaveEntry();
                    ToggleSpecialBoxes();
                });
        }

        private void LateUpdate()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.P))
            {
                _mapController.ChangeMap(_mapController.CurrentBonusMap());
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _mapController.ChangeMap(_maps.VictorBossFight);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {

                var UTCTime = Il2CppSystem.DateTime.UtcNow;
                double currentUTCUnixTimeStamp = TimeManager.GetUnixTimeStampFromDate(UTCTime);

                Plugin.Logger.Msg($"Current UTC Time: {UTCTime}");
                Plugin.Logger.Msg($"Purple box last used: {TimeManager.GetDateTime(_mapController.specialRandomBoxLastUsed)}");

                Plugin.Logger.Msg($"Current UTC Unix Timestamp: {currentUTCUnixTimeStamp}");
                Plugin.Logger.Msg($"Purple box last used unix: {_mapController.specialRandomBoxLastUsed}");

                Plugin.Logger.Msg($"Purple box Chance: {_playerInventory.specialRandomBoxChance}");

            }

#endif

            if (ModEnabled && Input.GetKeyDown(Plugin.Config.SpecialBoxesToggleKey.Value))
            {
                ToggleSetting("Special Boxes", ref _specialBoxesEnabled, Plugin.Config.SpecialBoxesShowPopup.Value);
                ToggleSpecialBoxes();
            }
        }

        private void ToggleSetting(string type, ref bool state, bool showPopup)
        {
            state = !state;

            Plugin.Logger.Msg($"{type} are: {(state ? "ON" : "OFF")}");
            if (showPopup)
                Plugin.ModHelperInstance.ShowNotification(state ? $"{type} enabled!" : $"{type} disabled!", state);

            Plugin.Config.NoSpecialBoxesEnabled.Value = state;
        }

        private void ToggleSpecialBoxes()
        {
            if (!ModEnabled)
            {
                // Mod switched off: back to the chance the game had when the mod started.
                _playerInventory.specialRandomBoxChance = _specialRandomBoxChanceBackup;
                return;
            }

            if (!_specialBoxesEnabled)
            {
                _playerInventory.specialRandomBoxChance = 0f;
            }
            else
            {
                var currentUTCTime = Il2CppSystem.DateTime.UtcNow;
                double currentUTCUnixTimeStamp = TimeManager.GetUnixTimeStampFromDate(currentUTCTime);

                if (currentUTCUnixTimeStamp - _mapController.specialRandomBoxLastUsed > 7200)
                {
                    _playerInventory.specialRandomBoxChance = 100f;
                    Plugin.Logger.Debug("100% chance");
                }
                else
                {
                    _playerInventory.specialRandomBoxChance = _specialRandomBoxChanceBackup;
                    Plugin.Logger.Debug("normal chance");
                }
            }
        }
        private const float SliderTimeout = 10f;

        public void SkipSlider(BonusStartSlider slider)
        {
            if (slider == null || !ModEnabled || !GameState.IsBonus()) return;

            MelonCoroutines.Start(ConfirmSliderWhenReady(slider));
        }

        // The slider is not ready yet when it is set up. Wait for it across frames;
        // spinning in a loop here would block the game's main thread and freeze it.
        private static IEnumerator ConfirmSliderWhenReady(BonusStartSlider slider)
        {
            var giveUpAt = Time.time + SliderTimeout;
            while (slider != null && !slider.sliderReady && Time.time < giveUpAt)
                yield return null;

            if (slider != null && slider.sliderReady)
                slider.confirmAction?.Invoke();
        }

        [HarmonyPatch(typeof(BonusStartSlider), "SetRandomPuzzle")]
        public class BonusStartSliderPatch
        {
            [HarmonyPostfix]
            static void Postfix(BonusStartSlider __instance)
            {
                NoSpecialBoxes.Instance?.SkipSlider(__instance);
                Plugin.Logger.Debug("Detected BonusStartSlider creation.");
            }
        }

    }
}


