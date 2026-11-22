using Il2Cpp;

namespace PerfectChestHunter;

/// <summary>
/// Which chest is the best pick. Shared by Perfect mode (which opens it) and the overlay (which highlights it).
/// </summary>
internal static class ChestPicks
{
    // MultiplierIncreaser was added to the game after the reference assemblies in /Libraries were
    // generated, so it is looked up by name: the mod builds against both and works on both.
    private static readonly ChestType? MultiplierIncreaser =
        Enum.TryParse<ChestType>("MultiplierIncreaser", out var type) ? type : null;

    internal static bool IsMultiplierIncreaser(ChestType type) => type == MultiplierIncreaser;

    /// <summary>The unopened Multiplier chest with the highest multiplier, or null.</summary>
    internal static Chest BestMultiplier(IEnumerable<Chest> chests) =>
        chests.Where(c => c.type == ChestType.Multiplier && ChestHunt.IsOpenable(c))
              .MaxBy(c => c.multiplier);

    /// <summary>A permanent upgrade if one is on offer, otherwise the highest multiplier. Null if none can be opened.</summary>
    internal static Chest BestPerfectChest(IEnumerable<Chest> perfectChests)
    {
        var candidates = perfectChests.Where(c => c != null && !c.opened && c.perfectChestObject != null).ToList();
        return candidates.FirstOrDefault(c => c.type == ChestType.PermanentUpgrade)
            ?? candidates.MaxBy(c => c.multiplier);
    }
}
