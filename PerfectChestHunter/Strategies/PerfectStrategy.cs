using System.Collections;
using Il2Cpp;

namespace PerfectChestHunter.Strategies;

/// <summary>
/// Opens every non-mimic chest so the round ends perfect, in the order that gives the highest
/// multiplier: increasers, then duplicators, (optionally armory chests), then the best multiplier, then the rest.
/// While the mimic achievement is still locked, it instead opens a mimic to unlock it.
/// </summary>
internal sealed class PerfectStrategy(Settings settings, bool unlockMimicAchievement) : IChestOpeningStrategy
{
    private const int MaxMimicsToOpen = 3;

    public IEnumerator OpenChests(ChestHunt hunt)
    {
        if (unlockMimicAchievement)
        {
            yield return OpenMimics(hunt);
            yield break;
        }

        yield return OpenAll(hunt, c => ChestPicks.IsMultiplierIncreaser(c.type));
        yield return OpenDuplicators(hunt);

        if (settings.MultiplyArmoryChests.Value)
            yield return OpenAll(hunt, c => c.type == ChestType.ArmoryChest);

        var best = ChestPicks.BestMultiplier(hunt.Chests);
        if (best != null)
            yield return hunt.Open(best, ChestHunt.DefaultOpenDelay);

        yield return OpenAll(hunt, c => c.type != ChestType.Mimic);
    }

    private static IEnumerator OpenMimics(ChestHunt hunt)
    {
        var mimics = hunt.Chests.Where(c => c.type == ChestType.Mimic && ChestHunt.IsOpenable(c));
        foreach (var mimic in mimics.Take(MaxMimicsToOpen))
            yield return hunt.Open(mimic, ChestHunt.DefaultOpenDelay);
    }

    /// <summary>Every duplicator after the first gets a long pause, so the previous duplication finishes first.</summary>
    private static IEnumerator OpenDuplicators(ChestHunt hunt)
    {
        var first = true;
        foreach (var chest in hunt.Chests.Where(c => c.type == ChestType.DuplicateNextPick && ChestHunt.IsOpenable(c)))
        {
            yield return hunt.Open(chest, first ? ChestHunt.DefaultOpenDelay : ChestHunt.SlowOpenDelay);
            first = false;
        }
    }

    private static IEnumerator OpenAll(ChestHunt hunt, Func<Chest, bool> predicate)
    {
        foreach (var chest in hunt.Chests.Where(predicate))
            yield return hunt.Open(chest, ChestHunt.DefaultOpenDelay);
    }
}
