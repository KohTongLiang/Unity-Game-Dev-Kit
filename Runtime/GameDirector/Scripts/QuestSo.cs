using System;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// At start time, QuestManager will retrieve all the Quest Scriptable Objects and generate a quest instance for
    /// each of it.
    /// </summary>
    [CreateAssetMenu(fileName = "Quest", menuName = "GameDirector/Quest", order = 1)]
    public class QuestSo : ScriptableObject
    {
        [NonSerialized] public bool isInitialized = false;
        [NonSerialized] public int QuestId;

        [Header("Basic Quest Information")]
        public string questAssetId; // id in editor mode, at runtime a ulong is generated
        public string questTitle;
        public string questDescription;

        [Header("Quest pre-requisites")]
        public QuestSo[] preRequisites;

        [Header("Quest objectives")] 
        public QuestObjectiveSo[] objectivesPrefab;

        /// <summary>
        /// Create a instance of the quest during runtime for the Quest Manager.
        /// </summary>
        /// <returns>Quest instance based on this quest Scriptable</returns>
        public Quest CreateQuest()
        {
            QuestId = questTitle.ComputeFNV1aHash();
            var quest = new Quest
            {
                // Setup quest information
                QuestId = QuestId,
                PreRequisites = preRequisites.Select(pre => pre.questAssetId).ToArray(),
                CurrentQuestState = preRequisites.Length > 0 ? QuestState.Locked : QuestState.Unlocked,
                questTitle = questTitle,
                questDescription = questDescription,
                ObjectivesDictionary = new(),
            };

            foreach (var questObjSo in objectivesPrefab)
            {
                QuestObjective newObj = questObjSo.CreateQuestObjective();
                quest.ObjectivesDictionary.Add(newObj.ObjectiveId, newObj);
            }

            return quest;
        }

        // TODO: Quest Rewards
#if UNITY_EDITOR
        private void OnValidate()
        {
            questAssetId = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}