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
        public int ObjectiveId;
        public string ObjectiveTitle;
        public string ObjectiveDescription;
        public QuestObjectiveSo Blueprint;

        public QuestState ObjectiveState;
        public int ObjectiveStepCount;
        public int CurrentObjectiveStep;

        public Quest QuestRef;
        public QuestObjectiveSo[] ExcludeObjectives;

        public delegate void OnObjectiveInitialisedEvent(QuestObjective questObjective);
        public delegate void OnObjectiveStartEvent(QuestObjective questObjective);
        public delegate void OnObjectiveUpdateEvent(QuestObjective questObjective);
        public delegate void OnObjectiveFailedEvent(QuestObjective questObjective);
        public delegate void OnObjectiveCompletedEvent(QuestObjective questObjective);

        public event OnObjectiveInitialisedEvent OnObjectiveInitialised;
        public event OnObjectiveStartEvent OnObjectiveStart;
        public event OnObjectiveUpdateEvent OnObjectiveUpdate;
        public event OnObjectiveFailedEvent OnObjectiveFailed;
        public event OnObjectiveCompletedEvent OnObjectiveCompleted;

        public virtual void InitialiseObjective(Quest quest)
        {
            ObjectiveStepCount = Blueprint.ObjectiveRepeatCount;
            CurrentObjectiveStep = 0;
            ObjectiveState = QuestState.Unlocked;
            OnObjectiveInitialised?.Invoke(this);
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
            CurrentObjectiveStep = 0;
            OnObjectiveStart?.Invoke(this);
        }

        /// <summary>
        /// Any relevant logic to update the objective
        /// </summary>
        public virtual void UpdateObjective()
        {
            // Update and check repeat count, if reached then completed
            CurrentObjectiveStep++;
            if (CurrentObjectiveStep >= ObjectiveStepCount)
            {
                CompleteObjective();
            }
            else
            {
                OnObjectiveUpdate?.Invoke(this);
            }
        }

        /// <summary>
        /// Fired when objective failed.
        /// </summary>
        public virtual void FailObjective()
        {
            ObjectiveState = QuestState.Failed;
            OnObjectiveFailed?.Invoke(this);
        }

        /// <summary>
        /// Fired when objective is completed
        /// </summary>
        public virtual void CompleteObjective()
        {
            QuestObjective objectiveRef = new();
            // Fail Excluded Objectives, making them uncompletable
            foreach (var excludedObjective in ExcludeObjectives)
            {
                QuestRef.QuestManagerRef.GetQuestObjective(QuestRef.QuestId, excludedObjective.QuestObjectiveId,
                    ref objectiveRef);
                objectiveRef.ObjectiveState = QuestState.Failed;
            }

            // Complete objective
            ObjectiveState = QuestState.Completed;
            OnObjectiveCompleted?.Invoke(this);
        }
    }
}