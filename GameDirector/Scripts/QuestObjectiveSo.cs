using System;
using UnityEditor;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Stateless Quest Objective logic
    /// </summary>
    [CreateAssetMenu(fileName = "QuestObjective", menuName = "GameDirector/QuestObjective", order = 1)]
    public class QuestObjectiveSo : ScriptableObject
    {
        [NonSerialized] public int QuestObjectiveId;

        public string QuestObjectiveAssetId;
        public string ObjectiveTitle;
        public string ObjectiveDescription;
        public int ObjectiveCount;
        
        public QuestObjective CreateQuestObjective()
        {
            QuestObjectiveId = QuestObjectiveAssetId.ComputeFNV1aHash();
            QuestObjective newObj = new()
            {
                // Setup objective information
                ObjectiveId = QuestObjectiveId,
                ObjectiveTitle = ObjectiveTitle,
                ObjectiveDescription = ObjectiveDescription,
                QuestObjectiveCount = ObjectiveCount
            };

            return newObj;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            QuestObjectiveAssetId = this.name;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}