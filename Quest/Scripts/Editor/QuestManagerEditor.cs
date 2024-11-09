using DeliveryGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore
{
    [CustomEditor(typeof(DeliveryGameQuestManager))]
    public class QuestManagerEditor : Editor
    {
        private int selectedQuestIndex = 0;
        private string[] questNames;
        private QuestSo[] allQuests;

        private void OnEnable()
        {
            // Load all QuestSo Scriptable Objects under the Assets/Resources/Quests folder
            allQuests = Resources.LoadAll<QuestSo>("Quests");

            // Create an array of quest names
            questNames = new string[allQuests.Length];
            for (int i = 0; i < allQuests.Length; i++)
            {
                questNames[i] = allQuests[i].name;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var questManager = target as DeliveryGameQuestManager;
            
            EditorGUILayout.LabelField("Quest Manager Editor Tools", EditorStyles.boldLabel);
            
            // Print active quests hashset from questManager
            EditorGUILayout.LabelField("Active Quests: ");
            if (questManager != null)
                foreach (var quest in questManager.QuestManager.ActiveQuests)
                {
                    EditorGUILayout.LabelField(quest.Key);
                }

            // Create a dropdown for selecting a quest
            selectedQuestIndex = EditorGUILayout.Popup("Select Quest", selectedQuestIndex, questNames);

            if (GUILayout.Button("Start Quest"))
            {
                if (selectedQuestIndex >= 0 && selectedQuestIndex < allQuests.Length)
                {
                    string questId = allQuests[selectedQuestIndex].questAssetId; // Assuming QuestSo has a QuestId property
                    EventManager.Instance.TriggerEvent(QuestEventConstants.StartQuestEvent, questId);
                }
            }
        }
    }
}