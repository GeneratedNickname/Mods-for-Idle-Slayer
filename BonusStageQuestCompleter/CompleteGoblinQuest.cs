using Il2Cpp;
using IdleSlayerMods.Common.Extensions;
using UnityEngine;
using HarmonyLib;

namespace BonusStageQuestCompleter;

public class CompleteGoblinQuest : MonoBehaviour
{
    public static CompleteGoblinQuest Instance { get; private set; }
    private Il2CppSystem.Collections.Generic.List<Quest> questList;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Plugin.ModHelperInstance?.CreateSettingsToggle(MyPluginInfo.PLUGIN_NAME, Plugin.Config.Enabled,
            _ => Plugin.Config.Enabled.SaveEntry());
    }

    public void LateUpdate()
    {
        if (!Plugin.Config.Enabled.Value) return;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            CompleteHyperModeQuest();
        }
    }

    private void CompleteHyperModeQuest()
    {
        questList = QuestsList.instance?.lastScrollListData;
        if (questList == null) return;

        foreach (Quest quest in questList)
        {
            if (quest.name == "quest_hyper_mode")
            {
                quest.AddProgress(quest.questGoal - quest.questCurrentGoal);
                Plugin.Logger.Msg("Hyper Mode Quest Completed");
                return;
            }
        }
    }
}