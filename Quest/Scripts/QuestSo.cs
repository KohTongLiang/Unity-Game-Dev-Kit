using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore
{
    /// <summary>
    /// At start time, QuestManager will retrieve all the Quest Scriptable Objects and generate a quest instance for
    /// each of it.
    /// </summary>
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
    public class QuestSo : ScriptableObject
    {
        [NonSerialized] public bool isInitialized = false;
        [NonSerialized] public Guid questId;
        
        [Header("Basic Quest Information")]
        public string questAssetId; // id in editor mode, at runtime a ulong is generated
        public string questTitle;
        public string questDescription;
        public GameObject questPrefab;
        
        [Header("Quest pre-requisites")]
        public QuestSo[] preRequisites;

        [Header("Quest objectives")] 
        public QuestObjectiveSo[] objectivesPrefab;

        /// <summary>
        /// Create a instance of the quest during runtime for the Quest Manager.
        /// </summary>
        /// <returns>Quest instance based on this quest Scriptable</returns>
        public Quest CreateQuest(out GameObject questObj)
        {
            questObj = Instantiate(questPrefab);
            var quest = questObj.GetComponent<Quest>();

            questId = Guid.NewGuid();
            // Setup quest information
            quest.QuestId = questId;
            quest.PreRequisites = preRequisites.Select(pre => pre.questAssetId).ToArray();
            quest.CurrentQuestState = preRequisites.Length > 0 ? QuestState.Locked : QuestState.Unlocked;
            quest.questTitle = questTitle;
            quest.questDescription = questDescription;
            quest.ObjectivesList = new();

            foreach (var questObjSo in objectivesPrefab)
            {
                QuestObjective newObj = questObjSo.CreateQuestObjective(questObj);
                quest.ObjectivesList.Add(newObj);
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