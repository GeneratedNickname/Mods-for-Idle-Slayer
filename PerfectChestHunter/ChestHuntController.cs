using System.Collections;
using Il2Cpp;
using IdleSlayerMods.Common.Extensions;
using MelonLoader;
using PerfectChestHunter.Overlay;
using PerfectChestHunter.Strategies;
using UnityEngine;

namespace PerfectChestHunter;

/// <summary>
/// Sits on the game's "Chest Hunt" object and follows the chest hunt lifecycle. For each round it
/// starts the opening strategy of the current mode (if any) and drives the overlay. Changing the mode
/// mid-round stops the running strategy and starts the new one on the chests that are left.
/// </summary>
public class ChestHuntController : MonoBehaviour
{
    private Settings _settings;
    private ChestHunt _hunt;
    private ChestOverlay _overlay;
    private MimicAchievement _mimicAchievement;

    private bool _roundActive;
    private ChestHuntMode? _roundMode; // mode the current round's coroutine was started for
    private object _roundCoroutine;

    private void Awake()
    {
        _settings = Plugin.Settings;
        _hunt = new ChestHunt(gameObject.GetComponentInChildren<ChestHuntManager>());
        _overlay = new ChestOverlay(_hunt, _settings);
        _mimicAchievement = new MimicAchievement();
    }

    private void Start() => CreateSettingsToggles();

    private void Update()
    {
        if (!_settings.Enabled.Value)
        {
            // Switched off: stop acting and remove the overlay. Switching back on mid-round picks up from here.
            if (_roundActive) EndRound();
            return;
        }

        HandleHotkeys();

        if (!GameState.IsChestHunt())
        {
            if (_roundActive) EndRound();
            return;
        }

        _roundActive = true;

        var mode = _settings.Mode;
        if (mode != _roundMode)
            StartRound(mode);

        if (mode == ChestHuntMode.Overlay || _settings.ShowOverlayWhileAutoOpening.Value)
            _overlay.Refresh();
        else
            _overlay.Clear();
    }

    private void OnDestroy()
    {
        StopRoundCoroutine();
        _overlay?.Clear();
    }

    //////////////////////////////////////////////////////////////// Round lifecycle

    private void StartRound(ChestHuntMode mode)
    {
        StopRoundCoroutine();
        _roundMode = mode;

        var strategy = CreateStrategy(mode);
        if (strategy != null)
            _roundCoroutine = MelonCoroutines.Start(PlayRound(strategy));
    }

    private void EndRound()
    {
        StopRoundCoroutine();
        _overlay.Clear();
        _roundActive = false;
        _roundMode = null;
    }

    private IChestOpeningStrategy CreateStrategy(ChestHuntMode mode) => mode switch
    {
        ChestHuntMode.Perfect => new PerfectStrategy(_settings, unlockMimicAchievement: !_mimicAchievement.IsUnlocked),
        ChestHuntMode.Random => new RandomStrategy(),
        _ => null,
    };

    private IEnumerator PlayRound(IChestOpeningStrategy strategy)
    {
        yield return _hunt.WaitForChests();
        yield return strategy.OpenChests(_hunt);
        yield return _hunt.Finish();
        _roundCoroutine = null;
    }

    private void StopRoundCoroutine()
    {
        if (_roundCoroutine == null) return;
        MelonCoroutines.Stop(_roundCoroutine);
        _roundCoroutine = null;
    }

    //////////////////////////////////////////////////////////////// Input & settings UI

    private void HandleHotkeys()
    {
        if (Input.GetKeyDown(_settings.ModeCycleKey.Value))
        {
            var next = _settings.Mode switch
            {
                ChestHuntMode.Perfect => ChestHuntMode.Random,
                ChestHuntMode.Random => ChestHuntMode.Overlay,
                _ => ChestHuntMode.Perfect,
            };
            _settings.Mode = next;
            Notify($"Chest Hunt mode: {next}", true);
        }

        if (Input.GetKeyDown(_settings.ArmoryChestMultiplyToggleKey.Value))
        {
            var enabled = _settings.MultiplyArmoryChests.Value = !_settings.MultiplyArmoryChests.Value;
            _settings.MultiplyArmoryChests.SaveEntry();
            Notify(enabled ? "Armory Chest Multiplying enabled!" : "Armory Chest Multiplying disabled!", enabled);
        }

#if DEBUG
        if (Input.GetKeyDown(KeyCode.O) && !GameState.IsChestHunt())
            _hunt.StartEvent();
#endif
    }

    private void CreateSettingsToggles()
    {
        var helper = Plugin.ModHelperInstance;
        if (helper == null) return;

        helper.CreateSettingsToggle(MyPluginInfo.PLUGIN_NAME, _settings.Enabled,
            _ => _settings.Enabled.SaveEntry());
        helper.CreateSettingsToggle("Auto-open Chests (off: Overlay only)", _settings.AutoOpenChests,
            _ => _settings.AutoOpenChests.SaveEntry());
        helper.CreateSettingsToggle("Open Chests Randomly/Perfectly", _settings.PerfectChestsHuntOn,
            _ => _settings.PerfectChestsHuntOn.SaveEntry());
        helper.CreateSettingsToggle("Chest Overlay while Auto-opening", _settings.ShowOverlayWhileAutoOpening,
            _ => _settings.ShowOverlayWhileAutoOpening.SaveEntry());
    }

    private static void Notify(string message, bool shine)
    {
        Plugin.Logger.Msg(message);
        Plugin.ModHelperInstance?.ShowNotification(message, shine);
    }
}
