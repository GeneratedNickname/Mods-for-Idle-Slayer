using Il2Cpp;

namespace PerfectChestHunter;

/// <summary>The "That was fun" achievement, which needs a mimic to be opened once.</summary>
internal sealed class MimicAchievement
{
    private const string AchievementName = "achievement_that_was_fun";

    private Achievement _achievement;

    /// <summary>True when unlocked, and also when the achievement can't be found (so mimics are never opened on purpose).</summary>
    internal bool IsUnlocked
    {
        get
        {
            _achievement ??= Find();
            return _achievement == null || _achievement.unlocked;
        }
    }

    private static Achievement Find()
    {
        var inventory = PlayerInventory.instance;
        if (inventory == null) return null;

        foreach (var achievement in inventory.achievements)
            if (achievement.name == AchievementName)
                return achievement;

        return null;
    }
}
