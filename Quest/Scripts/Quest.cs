using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Quest : MonoBehaviour
    {
        // Basic quest information
        public string QuestId;
        public QuestState CurrentQuestState;
        public string questTitle;
        public string questDescription;

        // Pre-Requisites before the quest can be unlocked
        public string[] PreRequisites;

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

        /// <summary>
        /// Steps within quest objective itself
        /// </summary>
        public int ObjectiveIndex
        {
            set => ObjectivesList[_objectiveStep].QuestObjectiveIndex = value;
            get => ObjectivesList[_objectiveStep].QuestObjectiveIndex;
        }

        public List<QuestObjective> ObjectivesList;
        public Dictionary<string, QuestObjective> ObjectivesDictionary;

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
                    Debug.Log($"Quest {QuestId} is locked");
                    return;
                }

                CurrentQuestState = QuestState.Unlocked;
            }
            Debug.Log($"Start {QuestId} Quest");
            _questManager.AddActiveQuests(this);

            CurrentQuestState = QuestState.InProgress;
            
            EventManager.Instance.TriggerEvent(QuestEventConstants.UpdateQuestUIInfoEvent, this);

            SetupQuestObjectives();
        }

        private void SetupQuestObjectives()
        {
            _objectiveStep = 0;
            foreach (var qObj in ObjectivesList)
            {
                qObj.InitialiseObjective(QuestId);
                qObj.ObjectiveCompleteCallback += UpdateQuestObjectiveCallback;
                qObj.ObjectiveStepUpdateCallback += UpdateObjectiveStepCallback;
            }

            if (_objectiveStep < ObjectivesList.Count)
            {
                ObjectivesList[_objectiveStep].InitialiseObjective(QuestId);
            }

            questUpdateCallback?.Invoke(this);
        }

        private void UpdateObjectiveStepCallback()
        {
            questUpdateCallback?.Invoke(this);
        }

        private void UpdateQuestObjectiveCallback()
        {
            _objectiveStep++;
            if (_objectiveStep < ObjectivesList.Count)
            {
                // Setup next objective(s)
                ObjectivesList[_objectiveStep].InitialiseObjective(QuestId);
                questUpdateCallback?.Invoke(this);
            }
            else
            {
                // Complete the quest once all objectives are met
                EndQuest();
            }
        }

        public QuestObjective GetCurrentObjectives()
        {
            return _objectiveStep < ObjectivesList.Count ? ObjectivesList[_objectiveStep] : null;
        }

        /// <summary>
        /// Cleans up and complete the quest
        /// </summary>
        public void EndQuest()
        {
            CurrentQuestState = QuestState.Completed;
            _objectiveStep = 0;
            _questManager.RemoveActiveQuests(this);

            foreach (var qObj in ObjectivesList)
            {
                qObj.ObjectiveCompleteCallback -= UpdateQuestObjectiveCallback;
                qObj.ObjectiveStepUpdateCallback -= UpdateObjectiveStepCallback;
            }

            EventManager.Instance.TriggerEvent(QuestEventConstants.ClearUIEvent, 0);
            questUpdateCallback?.Invoke(this);
            Debug.Log("End Quest");
        }
    }
}