using System.IO;
using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemSo : ScriptableObject
    {
        [Header("Basic Item Information")]
        public int ItemAssetId;
        public string ItemAssetIdName;
        public string ItemName;
        public string ItemDescription;
        public Sprite ItemIcon;
        public string ItemIconResourcePath;
        public GameObject ItemPrefab;

        #if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (ItemIcon != null) ItemIconResourcePath = GetResourcesPath(UnityEditor.AssetDatabase.GetAssetPath(ItemIcon));
            ItemAssetId = ItemAssetIdName.ComputeFNV1aHash();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        private string GetResourcesPath(string fullPath)
        {
            // Check if the path contains a "Resources" folder
            int resourcesIndex = fullPath.IndexOf("/Resources/");
            if (resourcesIndex == -1)
            {
                Debug.LogError("Sprite must be inside a Resources folder!");
                return "";
            }

            // Extract the part after "Resources/"
            string resourcesRelativePath = fullPath.Substring(resourcesIndex + "/Resources/".Length);

            // Remove file extension (e.g., ".png")
            resourcesRelativePath = Path.ChangeExtension(resourcesRelativePath, null);

            return resourcesRelativePath;
        }
        
        #endif
    }
}