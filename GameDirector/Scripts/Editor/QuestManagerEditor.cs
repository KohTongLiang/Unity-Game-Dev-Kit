using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore
{
    [CustomEditor(typeof(QuestManager))]
    public class QuestManagerEditor : Editor
    {
        private QuestManager manager;
        public override VisualElement CreateInspectorGUI()
        {
            manager = target as QuestManager;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying || manager == null) return;

            if (GUILayout.Button("Load Quests"))
            {
                manager.LoadQuests();
            }

            EditorGUILayout.LabelField("Active Quests");
            var activeQuests = manager.ActiveQuests; // Copy to prevent runtime modification error
            foreach (var quest in activeQuests.Values)
            {
                GUILayout.Label(quest.questTitle);

                foreach (var objective in quest.ObjectivesDictionary.Values)
                {
                    GUILayout.Label($"\t{objective.ObjectiveTitle} {objective.ObjectiveState}");
                }

                if (GUILayout.Button("End"))
                {
                    manager.EndQuest(quest.QuestId);
                }
            }

            var questsLookup = manager.QuestMap;
            EditorGUILayout.LabelField("Quest List");
            foreach (var quest in questsLookup.Values)
            {
                GUILayout.Label(quest.questTitle);
                if (GUILayout.Button("Start"))
                {
                    manager.StartQuest(quest.QuestId, null);
                }
            }
        }
    }
}