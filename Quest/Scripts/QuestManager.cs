using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class QuestManager : Singleton<QuestManager>
    {
        // Stores all the quests available during the game runtime
        private readonly Dictionary<Guid, Quest> _questMap = new();
        public Dictionary<Guid, Quest> QuestMap => _questMap;

        private readonly Dictionary<Guid, Quest> _activeQuests = new();
        public Dictionary<Guid, Quest> ActiveQuests => _activeQuests;

        private readonly Dictionary<Guid, GameObject> questObjList = new();

        public delegate void QuestStartedEvent(Quest quest);
        public QuestStartedEvent OnQuestStarted;

        public delegate void QuestEndedEvent(Quest quest);
        public QuestEndedEvent OnQuestEnded;

        public delegate void QuestUpdatedEvent(Quest quest);
        public QuestUpdatedEvent OnQuestUpdated;

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
                _questMap.TryAdd(questInfo.questId, quest);
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
            if (questId is not Guid questIdStr) return null;
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
            OnQuestUpdated?.Invoke(quest);
        }

        private void QuestUpdated(Quest quest)
        {
            OnQuestUpdated?.Invoke(quest);
            if (quest.CurrentQuestState == QuestState.Completed)
            {
                QuestEnded(quest);
            }
        }

        private void QuestEnded(Quest quest)
        {
            OnQuestEnded?.Invoke(quest);
        }

        public void StartQuest(Guid questId, Action<Quest> questUpdateCallback)
        {
            _questMap.TryGetValue(questId, out var quest);
            if (quest != null)
            {
                quest.questUpdateCallback += questUpdateCallback;
                quest.questUpdateCallback += QuestUpdated;
                quest.StartQuest();
                OnQuestStarted?.Invoke(quest);
            }
        }

        public void EndQuest(Guid questId)
        {
            if (!_questMap.TryGetValue(questId, out var questInfo)) return;

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