using Il2Cpp;
using UnityEngine;

namespace PerfectChestHunter.Overlay;

/// <summary>What the overlay writes next to a chest: a short name for its contents and a colour.</summary>
internal readonly record struct ChestLabel(string Text, Color Color)
{
    private static readonly Color Red = new(1f, 0.3f, 0.3f);
    private static readonly Color Gold = new(1f, 0.84f, 0.2f);
    private static readonly Color Orange = new(1f, 0.6f, 0.2f);
    private static readonly Color Cyan = new(0.3f, 0.9f, 1f);
    private static readonly Color Violet = new(0.75f, 0.5f, 1f);
    private static readonly Color Green = new(0.4f, 1f, 0.4f);
    private static readonly Color Yellow = new(1f, 1f, 0.5f);
    private static readonly Color Blue = new(0.5f, 0.7f, 1f);
    private static readonly Color Pink = new(1f, 0.5f, 0.85f);

    /// <param name="best">Marks the chest that Perfect mode would open first (best multiplier / best perfect chest).</param>
    internal static ChestLabel For(Chest chest, bool best)
    {
        var label = Describe(chest);
        return best ? label with { Text = $"<b>{label.Text}</b> (best)" } : label;
    }

    private static ChestLabel Describe(Chest chest)
    {
        var type = chest.type;
        if (ChestPicks.IsMultiplierIncreaser(type))
            return new("+Mult", Orange);

        return type switch
        {
            ChestType.Mimic => new("MIMIC", Red),
            ChestType.Multiplier => new($"x{chest.multiplier}", Gold),
            ChestType.DuplicateNextPick => new("Dupe", Cyan),
            ChestType.ArmoryChest => new("Armory", Violet),
            ChestType.Saver => new("Saver", Green),
            ChestType.CPS => new("Coins", Yellow),
            ChestType.SlayerPoints => new("SP", Blue),
            ChestType.Both => new("Coins+SP", Yellow),
            ChestType.PermanentUpgrade => new("Upgrade", Pink),
            ChestType.Special => new("Special", Pink),
            _ => new(type.ToString(), Color.white),
        };
    }
}
