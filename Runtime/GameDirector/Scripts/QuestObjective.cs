using System;

namespace GameCore
{
    /// <summary>
    /// Objective is essentially the steps within the quest that needs to be done. There could be only one step per objective
    /// or multiple ones when the steps can be done in any order.
    /// </summary>
    [Serializable]
    public class QuestObjective
    {
        private ulong _stepId;
        public int ObjectiveId;
        public string ObjectiveTitle;
        public string ObjectiveDescription;
        public QuestState ObjectiveState;

        public int QuestObjectiveCount;
        protected int _questObjectiveIndex;
        public Quest parentQuest;

        public delegate void OnObjectiveInitialisedEvent(int QuestObjectiveId);
        public delegate void OnObjectiveStartEvent(int QuestObjectiveId);
        public delegate void OnObjectiveUpdateEvent(int QuestObjectiveId);
        public delegate void OnObjectiveFailedEvent(int QuestObjectiveId);
        public delegate void OnObjectiveCompletedEvent(int QuestObjectiveId);

        public event OnObjectiveInitialisedEvent OnObjectiveInitialised;
        public event OnObjectiveStartEvent OnObjectiveStart;
        public event OnObjectiveUpdateEvent OnObjectiveUpdate;
        public event OnObjectiveFailedEvent OnObjectiveFailed;
        public event OnObjectiveCompletedEvent OnObjectiveCompleted;

        public virtual void InitialiseObjective(Quest quest)
        {
            ObjectiveState = QuestState.Unlocked;
            parentQuest = quest;
            OnObjectiveInitialised?.Invoke(ObjectiveId);
        }

        /// <summary>
        /// Objective start
        /// </summary>
        public virtual void StartObjective()
        {
            if (ObjectiveState != QuestState.Unlocked)
            {
                return;
            }

            ObjectiveState = QuestState.InProgress;
            _questObjectiveIndex = QuestObjectiveCount;
            OnObjectiveStart?.Invoke(ObjectiveId);
        }

        /// <summary>
        /// Any relevant logic to update the objective
        /// </summary>
        public virtual void UpdateObjective()
        {
            OnObjectiveUpdate?.Invoke(ObjectiveId);
        }

        /// <summary>
        /// Fired when objective is completed
        /// </summary>
        public virtual void FailObjective()
        {
            ObjectiveState = QuestState.Failed;
            OnObjectiveFailed?.Invoke(ObjectiveId);
        }

        /// <summary>
        /// Fired when objective is completed
        /// </summary>
        public virtual void CompleteObjective()
        {
            ObjectiveState = QuestState.Completed;
            OnObjectiveCompleted?.Invoke(ObjectiveId);
        }
    }
}