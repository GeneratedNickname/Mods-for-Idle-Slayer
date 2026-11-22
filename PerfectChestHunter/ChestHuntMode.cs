namespace PerfectChestHunter;

internal enum ChestHuntMode
{
    /// <summary>Auto-opens every non-mimic chest in the best order, then picks the best perfect chest.</summary>
    Perfect,

    /// <summary>Auto-opens chests in a random order (mimics included) and takes whatever happens.</summary>
    Random,

    /// <summary>Never acts. Only labels each chest with its contents, next to the chest icon.</summary>
    Overlay,
}
