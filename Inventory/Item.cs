using UnityEngine;

namespace GameCore
{
    public class Item : MonoBehaviour
    {
        [Header("Do not set here, runtime assigned")]
        public int ItemRuntimeId = -1;
        public int ItemAssetId;

        [Header("Configuration for Item here if not using Scriptable Object")]
        public string ItemAssetIdName;
        public string ItemName;
        public string ItemDescription;

        protected virtual void Awake()
        {
            if (ItemRuntimeId == -1)
            {
                ItemFactory.Instance.RegisterItem(this);
            }
        }

        #if UNITY_EDITOR

        private void OnValidate()
        {
            ItemRuntimeId = -1;
            ItemAssetId = ItemAssetIdName.ComputeFNV1aHash();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        #endif
    }
}