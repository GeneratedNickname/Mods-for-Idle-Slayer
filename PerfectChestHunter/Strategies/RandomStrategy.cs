using System.Collections;
using Il2Cpp;

namespace PerfectChestHunter.Strategies;

/// <summary>Opens chests in a random order, mimics included, until the round ends.</summary>
internal sealed class RandomStrategy : IChestOpeningStrategy
{
    private static readonly System.Random Rng = new();

    public IEnumerator OpenChests(ChestHunt hunt)
    {
        var chests = hunt.Chests;
        Shuffle(chests);

        var lastWasDuplicator = false;
        foreach (var chest in chests)
        {
            if (!ChestHunt.IsOpenable(chest)) continue;

            // Pause after a mimic (round may end) and after a chained duplicator (let it resolve).
            var isDuplicator = chest.type == ChestType.DuplicateNextPick;
            var slow = chest.type == ChestType.Mimic || (isDuplicator && lastWasDuplicator);

            yield return hunt.Open(chest, slow ? ChestHunt.SlowOpenDelay : ChestHunt.DefaultOpenDelay);
            lastWasDuplicator = isDuplicator;

            if (!hunt.EventInProgress) yield break;
        }
    }

    private static void Shuffle<T>(IList<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
