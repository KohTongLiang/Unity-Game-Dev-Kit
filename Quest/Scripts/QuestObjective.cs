using System;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Objective is essentially the steps within the quest that needs to be done. There could be only one step per objective
    /// or multiple ones when the steps can be done in any order.
    /// </summary>
    [Serializable]
    public abstract class QuestObjective : MonoBehaviour
    {
        private ulong _stepId;
        public string QuestObjectiveId;
        public string ObjectiveTitle;
        public string ObjectiveDescription;
        public QuestObjectiveState ObjectiveState = QuestObjectiveState.Inactive;
        public Action ObjectiveCompleteCallback;
        public Action ObjectiveStepUpdateCallback;
        public Guid questId;

        public int questObjectiveCount;
        protected int _questObjectiveIndex;

        public int QuestObjectiveIndex
        {
            set => _questObjectiveIndex = value;
            get => _questObjectiveIndex;
        }

        public int QuestStep => questObjectiveCount - _questObjectiveIndex;

        // todo: Handle repeatable objectives properly
        protected QuestObjective(string objectiveTitle, string objectiveDescription)
        {
            ObjectiveTitle = objectiveTitle;
            ObjectiveDescription = objectiveDescription;
        }

        public virtual void InitialiseObjective(Quest quest)
        {
            ObjectiveState = QuestObjectiveState.Active;
            this.questId = quest.QuestId;
        }

        /// <summary>
        /// Objective start
        /// </summary>
        public virtual void StartObjective()
        {
            Debug.Log($"Objective {ObjectiveTitle} started");
            if (ObjectiveState != QuestObjectiveState.Active) return;
            ObjectiveState = QuestObjectiveState.InProgress;
            _questObjectiveIndex = questObjectiveCount;
        }

        /// <summary>
        /// Any relevant logic to update the objective
        /// </summary>
        public virtual void UpdateObjective(int objectiveIndex)
        {
        }

        /// <summary>
        /// Fired when objective is completed
        /// </summary>
        public virtual void CompleteObjective()
        {
            if (ObjectiveState != QuestObjectiveState.InProgress) return;
            Debug.Log($"Objective {ObjectiveTitle} ended");
            ObjectiveState = QuestObjectiveState.Inactive;
            ObjectiveCompleteCallback?.Invoke();
            ObjectiveCompleteCallback = null;
            // WaypointManager.Instance.CleanUp();
        }
    }
}