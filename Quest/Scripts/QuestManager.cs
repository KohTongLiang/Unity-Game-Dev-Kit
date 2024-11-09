using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class QuestManager : Singleton<QuestManager>
    {
        // Stores all the quests available during the game runtime
        private readonly Dictionary<string, Quest> _questMap = new();

        private readonly Dictionary<string, Quest> _activeQuests = new();
        public Dictionary<string, Quest> ActiveQuests => _activeQuests;

        /// <summary>
        /// TODO: Account for multiple ongoing quests
        /// </summary>
        private Quest _activeQuest;

        private readonly Dictionary<string, GameObject> questObjList = new();

        private void CreateQuestDictionary()
        {
            // loads all QuestInfoSO Scriptable Objects under the Assets/Resources/Quests folder
            QuestSo[] allQuests = Resources.LoadAll<QuestSo>("Quests");

            // Create the quest map
            foreach (QuestSo questInfo in allQuests)
            {
                Quest quest = questInfo.CreateQuest(out var questObj);
                questObjList.Add(quest.QuestId, questObj);

                quest.InitialiseQuest(this);
                _questMap.TryAdd(questInfo.questAssetId, quest);
            }

            Debug.Log($"There are {_questMap.Count} quests loaded");
        }

        public void LoadQuests()
        {
            CreateQuestDictionary();
        }

        public void ClearQuests()
        {
            ActiveQuests.Clear();
            _questMap.Clear();
        }

        public void QuestUpdateCallback(Quest quest)
        {

        }

        #region QuestHelpers

        /// <summary>
        /// Query pre-requisite quests for completion
        /// </summary>
        /// <param name="preRequisiteId"></param>
        /// <returns></returns>
        public bool QueryPreRequisite(string[] preRequisiteId)
        {
            return preRequisiteId.All(questId => GetQuest(questId)?.CurrentQuestState == QuestState.Completed);
        }

        public Quest GetQuest(object questId)
        {
            if (questId is not string questIdStr) return null;
            _questMap.TryGetValue(questIdStr, out var quest);
            if (quest != null)
            {
                return quest;
            }

            return null;
        }

        public void AddActiveQuests(Quest quest)
        {
            _activeQuests.TryAdd(quest.QuestId, quest);
        }

        public void RemoveActiveQuests(Quest quest)
        {
            _activeQuests.Remove(quest.QuestId);
        }

        public void StartQuest(string questId, Action<Quest> questUpdateCallback)
        {
            _questMap.TryGetValue(questId, out var quest);
            if (quest != null)
            {
                quest.questUpdateCallback = questUpdateCallback;
                quest.StartQuest();
            }
        }

        public void EndQuest(string quest)
        {
            if (!_questMap.TryGetValue(quest, out var questInfo)) return;

            if (questObjList.TryGetValue(questInfo.QuestId, out var questObj))
            {
                Destroy(questObj);
                questObjList.Remove(questInfo.QuestId);
            }

            questInfo.EndQuest();
        }

        #endregion
    }
}