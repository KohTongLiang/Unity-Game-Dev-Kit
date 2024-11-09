using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameCore
{
    public static class QuestCreator
    {
        private static ulong idCounter = 0u;
        private static readonly Queue<ulong> spareIds = new();
        
        public static void AssignQuestId(QuestSo quest)
        {
            if (quest == null || quest.isInitialized) return;

            // Load all QuestSo assets to find the next available ID
            var allQuests = AssetDatabase.FindAssets("t:QuestSo")
                .Select(guid => AssetDatabase.LoadAssetAtPath<QuestSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
            
            var a = allQuests.Find(q => q.questAssetId == quest.questAssetId);
            if (a != null)
            {
                Debug.LogWarning("Duplicate ID found when assigning quest id: " + quest.questAssetId);
                return;
            }

            quest.isInitialized = true;

            EditorUtility.SetDirty(quest);
            AssetDatabase.SaveAssets();
        }
    }
}