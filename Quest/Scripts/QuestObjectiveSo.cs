using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Stateless Quest Objective logic
    /// </summary>
    [CreateAssetMenu(fileName = "QuestObjective", menuName = "ScriptableObjects/QuestObjective", order = 1)]
    public class QuestObjectiveSo : ScriptableObject
    {
        public string QuestObjectiveAssetId;
        public string ObjectiveTitle;
        public string ObjectiveDescription;
        public GameObject questObjectivePrefab;
        public int ObjectiveCount;
        
        [NonSerialized] private QuestObjectiveState ObjectiveState = QuestObjectiveState.Inactive;
        [NonSerialized] public Action ObjectiveCompleteCallback;

        public QuestObjective CreateQuestObjective(GameObject questObj)
        {
            var newObjGo = Instantiate(questObjectivePrefab, questObj.transform);
            var newObj = newObjGo.GetComponent<QuestObjective>();

            // Setup objective information
            newObj.ObjectiveTitle = ObjectiveTitle;
            newObj.ObjectiveDescription = ObjectiveDescription;
            newObj.QuestObjectiveId = QuestObjectiveAssetId;
            newObj.questObjectiveCount = ObjectiveCount;
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