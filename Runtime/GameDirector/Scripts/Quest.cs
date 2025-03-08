using System;
using System.Collections.Generic;
using System.Linq;

namespace GameCore
{
    public class Quest
    {
        // Basic quest information
        public int QuestId;
        public QuestState CurrentQuestState;
        public string questTitle;
        public string questDescription;

        public delegate void OnQuestStartEvent(Quest quest);
        public delegate void OnQuestUpdateEvent(Quest quest);
        public delegate void OnQuestFailedEvent(Quest quest);
        public delegate void OnQuestCompletedEvent(Quest quest);

        public event OnQuestStartEvent OnQuestStart;
        public event OnQuestUpdateEvent OnQuestUpdate;
        public event OnQuestFailedEvent OnQuestFailed;
        public event OnQuestCompletedEvent OnQuestCompleted;

        // Pre-Requisites before the quest can be unlocked
        public int[] PreRequisites;

        // "Objectives"
        private int _objectiveStep = 0;

        /// <summary>
        /// Quest Objective Steps
        /// </summary>
        public int ObjectiveStep
        {
            get => _objectiveStep;
            set => _objectiveStep = value;
        }

        public Dictionary<int, QuestObjective> ObjectivesDictionary;
        private QuestManager _questManager;
        public Action<Quest> questUpdateCallback;

        public void InitialiseQuest(QuestManager questManager)
        {
            _questManager = questManager;
        }

        public void StartQuest()
        {
            if (CurrentQuestState == QuestState.Locked)
            {
                if (!_questManager.QueryPreRequisite(PreRequisites))
                {
                    return;
                }

                CurrentQuestState = QuestState.Unlocked;
            }

            _questManager.AddActiveQuests(this);
            CurrentQuestState = QuestState.InProgress;
            SetupQuestObjectives();

            // Start the first objective
            ObjectivesDictionary.Values.ToList()[0]?.StartObjective();
            OnQuestStart?.Invoke(this);
        }

        private void OnObjectiveStarted(int objectiveId)
        {
            questUpdateCallback?.Invoke(this);
        }

        private void OnObjectiveCompleted(int objectiveId)
        {
            if (CheckQuestComplete()) EndQuest();
            else questUpdateCallback?.Invoke(this);
        }

        public void GetObjective(int objectiveId, ref QuestObjective objective)
        {
            ObjectivesDictionary.TryGetValue(objectiveId, out objective);
        }

        private void SetupQuestObjectives()
        {
            _objectiveStep = 0;
            foreach (var qObj in ObjectivesDictionary.Values)
            {
                qObj.InitialiseObjective(this);
                qObj.OnObjectiveStart += OnObjectiveStarted;
                qObj.OnObjectiveCompleted += OnObjectiveCompleted;
            }

            questUpdateCallback?.Invoke(this);
        }

        private void ClearQuestObjectives()
        {
            foreach (var qObj in ObjectivesDictionary.Values)
            {
                qObj.OnObjectiveStart -= OnObjectiveStarted;
                qObj.OnObjectiveCompleted -= OnObjectiveCompleted;
            }
        }

        /// <summary>
        /// Dynamically update quest objective's repeat count.
        /// </summary>
        /// <param name="objectiveId"></param>
        public void UpdateObjectiveCount(int objectiveId)
        {
            if (ObjectivesDictionary.TryGetValue(objectiveId, out var objective))
            {
                objective.ObjectiveRepeatCount++;
                questUpdateCallback?.Invoke(this);
                ObjectivesDictionary[objectiveId] = objective;
            }
        }

        private bool CheckQuestComplete()
        {
            foreach (var objective in ObjectivesDictionary.Values)
            {
                if (objective.ObjectiveState != QuestState.Completed)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Cleans up and complete the quest
        /// </summary>
        public void EndQuest()
        {
            ClearQuestObjectives();
            CurrentQuestState = QuestState.Completed;
            _objectiveStep = 0;
            _questManager.RemoveActiveQuests(this);
            questUpdateCallback?.Invoke(this);
            questUpdateCallback = null;
        }
    }
}