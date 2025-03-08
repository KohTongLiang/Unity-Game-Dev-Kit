using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    public class QuestManager : Singleton<QuestManager>
    {
        [Header("Folder in Resource folder where Quests Scriptable Objects are Stored")]
        [SerializeField] private string questPath = "Quests";

        [SerializeField] private bool loadQuestsOnStart;

        // Stores all the quests available during the game runtime
        private readonly Dictionary<int, Quest> _questMap = new();
        public Dictionary<int, Quest> QuestMap => _questMap;

        private readonly Dictionary<int, Quest> _activeQuests = new();
        public Dictionary<int, Quest> ActiveQuests => _activeQuests;

        public delegate void QuestStartedEvent(Quest quest);
        public QuestStartedEvent OnQuestStarted;

        public delegate void QuestEndedEvent(Quest quest);
        public QuestEndedEvent OnQuestEnded;

        public delegate void QuestUpdatedEvent(Quest quest);
        public QuestUpdatedEvent OnQuestUpdated;

        private void Awake()
        {
            ServiceLocator.Global.Register(this);
        }

        private void Start()
        {
            if (loadQuestsOnStart) LoadQuests();
        }

        private void CreateQuestDictionary()
        {
            // loads all QuestInfoSO Scriptable Objects under the Assets/Resources/Quests folder
            QuestSo[] allQuests = Resources.LoadAll<QuestSo>("Quests");

            // Create the quest map
            foreach (QuestSo questInfo in allQuests)
            {
                Quest quest = questInfo.CreateQuest();
                quest.InitialiseQuest(this);
                _questMap.TryAdd(quest.QuestId, quest);
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
        }

        public void QuestUpdateCallback(Quest quest)
        {

        }

        #region Runtime Quest Utilities

        /// <summary>
        /// Query pre-requisite quests for completion
        /// </summary>
        /// <param name="preRequisiteId"></param>
        /// <returns></returns>
        public bool QueryPreRequisite(int[] preRequisiteId)
        {
            return preRequisiteId.All(questId =>
            {
                Quest quest = new();
                GetQuest(questId, ref quest);
                return quest?.CurrentQuestState == QuestState.Completed;
            });
        }

        public bool GetQuest(int questId, ref Quest quest)
        {
            _questMap.TryGetValue(questId, out quest);
            return quest != null;
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

        public bool StartQuest(int questId, ref Quest quest, Action<Quest> questUpdateCallback = null)
        {
            if (_questMap.TryGetValue(questId, out quest) && quest.CurrentQuestState != QuestState.InProgress)
            {
                quest.questUpdateCallback += questUpdateCallback;
                quest.questUpdateCallback += QuestUpdated;
                quest.StartQuest();
                OnQuestStarted?.Invoke(quest);
                return true;
            }

            return false;
        }

        public void EndQuest(int questId)
        {
            if (!_questMap.TryGetValue(questId, out var questInfo)) return;
            questInfo.EndQuest();
        }

        public void GetQuestObjective(int questId, int objectiveId, ref QuestObjective objective)
        {
            _questMap.TryGetValue(questId, out var quest);
            if (quest == null) return;
            quest.GetObjective(objectiveId, ref objective);
        }

        #endregion
    }
}