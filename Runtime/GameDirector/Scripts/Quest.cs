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

        public QuestManager QuestManagerRef;

        public delegate void OnQuestStartEvent(Quest quest);
        public delegate void OnQuestUpdateEvent(Quest quest, QuestObjective objective);
        public delegate void OnQuestFailedEvent(Quest quest);
        public delegate void OnQuestCompletedEvent(Quest quest);

        public event OnQuestStartEvent OnQuestStart;
        public event OnQuestUpdateEvent OnQuestUpdate;
        public event OnQuestFailedEvent OnQuestFailed;
        public event OnQuestCompletedEvent OnQuestCompleted;

        // Pre-Requisites before the quest can be unlocked
        public int[] PreRequisites;

        public Dictionary<int, QuestObjective> ObjectivesDictionary;
        private QuestManager _questManager;

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

        private void OnObjectiveStarted(QuestObjective objective)
        {
            OnQuestUpdate?.Invoke(this, objective);
        }

        private void OnObjectiveUpdated(QuestObjective objective)
        {
            OnQuestUpdate?.Invoke(this, objective);
        }

        private void OnObjectiveCompleted(QuestObjective objective)
        {
            ObjectivesDictionary[objective.ObjectiveId] = objective;
            if (CheckQuestComplete())
            {
                EndQuest();
            }
            else
            {
                OnQuestUpdate?.Invoke(this, objective);
            }
        }

        private bool CheckQuestComplete()
        {
            foreach (var objective in ObjectivesDictionary.Values)
            {
                if (objective.ObjectiveState != QuestState.Completed && objective.ObjectiveState != QuestState.Failed)
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
            CurrentQuestState = QuestState.Completed;
            _questManager.RemoveActiveQuests(this);
            OnQuestCompleted?.Invoke(this);
            ClearQuestObjectives();
        }

        #region Quest's Objective Utility Functions

        public int GetCompletedObjectivesCount()
        {
            return ObjectivesDictionary.Count(x => x.Value.ObjectiveState == QuestState.Completed);
        }

        public void GetObjective(int objectiveId, ref QuestObjective objective)
        {
            ObjectivesDictionary.TryGetValue(objectiveId, out objective);
        }

        private void SetupQuestObjectives()
        {
            foreach (var qObj in ObjectivesDictionary.Values)
            {
                qObj.InitialiseObjective(this);
                qObj.OnObjectiveStart += OnObjectiveStarted;
                qObj.OnObjectiveUpdate += OnObjectiveUpdated;
                qObj.OnObjectiveCompleted += OnObjectiveCompleted;
            }
        }

        private void ClearQuestObjectives()
        {
            foreach (var qObj in ObjectivesDictionary.Values)
            {
                qObj.OnObjectiveStart -= OnObjectiveStarted;
                qObj.OnObjectiveUpdate -= OnObjectiveUpdated;
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
                objective.ObjectiveStepCount++;
                ObjectivesDictionary[objectiveId] = objective;
            }
        }

        #endregion
    }
}