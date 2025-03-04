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
        public GameObject ItemPrefab;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            ItemAssetId = ItemAssetIdName.ComputeFNV1aHash();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}