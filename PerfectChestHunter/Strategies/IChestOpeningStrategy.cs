using System.Collections;

namespace PerfectChestHunter.Strategies;

/// <summary>
/// Opens the regular chests of one round. Waiting for the round to start and finishing it
/// (perfect chest, close button) are handled by the controller, the same way for every strategy.
/// </summary>
internal interface IChestOpeningStrategy
{
    IEnumerator OpenChests(ChestHunt hunt);
}
