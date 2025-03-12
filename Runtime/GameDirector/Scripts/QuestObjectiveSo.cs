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

        [Min(0)]
        [Tooltip("The number of times, this objectives need to be repeated for it to be counted as complete. Can be set to 0 if its set dynamically.")]
        public int ObjectiveRepeatCount;
        
        public QuestObjective CreateQuestObjective()
        {
            QuestObjectiveId = QuestObjectiveAssetId.ComputeFNV1aHash();
            QuestObjective newObj = new()
            {
                // Setup objective information
                ObjectiveId = QuestObjectiveId,
                ObjectiveTitle = ObjectiveTitle,
                ObjectiveDescription = ObjectiveDescription,
                ObjectiveStepCount = ObjectiveRepeatCount,
                Blueprint = this
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