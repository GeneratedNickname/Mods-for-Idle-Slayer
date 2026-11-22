using System.Collections;
using Il2Cpp;
using UnityEngine;

namespace PerfectChestHunter;

/// <summary>
/// Thin wrapper around the game's <see cref="ChestHuntManager"/>: state queries plus the basic
/// actions (open a chest, finish the round) that the opening strategies are built from.
/// </summary>
internal sealed class ChestHunt(ChestHuntManager manager)
{
    internal const float DefaultOpenDelay = 0.25f;
    internal const float SlowOpenDelay = 3f;

    internal bool EventInProgress => manager.eventInProgress;

    /// <summary>The round's chests, copied into a managed list so callers can iterate and sort freely.</summary>
    internal List<Chest> Chests => Snapshot(manager.chests);

    internal List<Chest> PerfectChests => Snapshot(manager.perfectChests);

    internal bool ChestsReady => manager.IsVisible() && manager.chests != null && manager.chests.Count > 0;

    /// <summary>
    /// True once the perfect-chest stage is on screen. The list can still hold the previous round's
    /// chests, whose objects are already destroyed, so every chest must have a live object.
    /// </summary>
    internal bool PerfectChestsReady
    {
        get
        {
            var list = manager.perfectChests;
            if (list == null || list.Count == 0) return false;
            for (var i = 0; i < list.Count; i++)
                if (list[i] == null || list[i].perfectChestObject == null) return false;
            return true;
        }
    }

    internal bool CloseButtonActive
    {
        get
        {
            var closeButton = manager.closeButton;
            if (closeButton == null) return false;
            var button = closeButton.GetComponent<AnimatedButton>();
            return button != null && button.isActiveAndEnabled;
        }
    }

    internal static bool IsOpenable(Chest chest) =>
        chest != null && !chest.opened && chest.chestObject != null;

    internal IEnumerator WaitForChests()
    {
        while (!ChestsReady)
            yield return null;
    }

    internal IEnumerator Open(Chest chest, float delayAfter)
    {
        if (!IsOpenable(chest)) yield break;

        Plugin.LogDebug($"Opening {chest.type} chest");
        chest.chestObject.Open(true);
        yield return new WaitForSeconds(delayAfter);
    }

    /// <summary>
    /// Ends the round after the strategy is done opening. A round ends in one of two ways: it is over
    /// (the close button shows), or every non-mimic chest was opened and the perfect-chest stage appears.
    /// In the second case the best perfect chest is opened first.
    /// </summary>
    internal IEnumerator Finish()
    {
        while (EventInProgress && !PerfectChestsReady)
            yield return null;

        while (!CloseButtonActive && !PerfectChestsReady)
            yield return null;

        if (PerfectChestsReady)
        {
            var pick = ChestPicks.BestPerfectChest(PerfectChests);
            if (pick != null)
            {
                Plugin.LogDebug($"Opening perfect chest: {pick.type} x{pick.multiplier}");
                pick.perfectChestObject.Open(true);
            }
        }

        while (!CloseButtonActive)
            yield return null;

        manager.Close();
    }

#if DEBUG
    internal void StartEvent() => manager.StartEvent();
#endif

    private static List<Chest> Snapshot(Il2CppSystem.Collections.Generic.List<Chest> source)
    {
        var result = new List<Chest>(source?.Count ?? 0);
        if (source == null) return result;
        for (var i = 0; i < source.Count; i++)
            result.Add(source[i]);
        return result;
    }
}
