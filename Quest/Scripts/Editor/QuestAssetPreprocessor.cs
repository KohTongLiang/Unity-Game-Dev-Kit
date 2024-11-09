using UnityEditor;

namespace GameCore
{
    public class QuestAssetPreprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                QuestSo quest = AssetDatabase.LoadAssetAtPath<QuestSo>(assetPath);
                if (quest != null)
                {
                    // QuestCreator.AssignQuestId(quest);
                }
            }
        }
    }
}