using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemSo : ScriptableObject
    {
        [Header("Basic Item Information")]
        public string itemAssetId; // id in editor mode, at runtime a ulong is generated
        public string itemName;
        public string itemDescription;
        public GameObject itemPrefab;

#if UNITY_EDITOR
        private void OnValidate()
        {
            itemAssetId = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}