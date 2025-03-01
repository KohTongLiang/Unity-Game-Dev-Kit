using System;
using UnityEngine;

namespace GameCore
{
    [Serializable]
    public class Item : MonoBehaviour
    {
        [Header("Do not set here, runtime assigned")]
        public int ItemRuntimeId = -1;
        public int ItemAssetId;

        [Header("Configuration for Item here if not using Scriptable Object")]
        [SerializeField] protected bool registerOnAwake = false;
        public string ItemAssetIdName;
        public string ItemName;
        public string ItemDescription;

        [Header("Blueprint Reference")]
        public ItemSo ItemBlueprint;

        protected virtual void Awake()
        {
            if (registerOnAwake)
            {
                ItemFactory.Instance.RegisterItem(this);
                UpdateItemAssetDetailsFromBlueprint();
            }
        }

        /// <summary>
        /// Update the item asset details from blueprint attached to the item. This is for items
        /// that are added directly into the scene instead of being created from the factory.
        /// </summary>
        public void UpdateItemAssetDetailsFromBlueprint()
        {
            ItemAssetId = ItemBlueprint.ItemAssetId;
            ItemAssetIdName = ItemBlueprint.ItemAssetIdName;
            ItemName = ItemBlueprint.ItemName;
            ItemDescription = ItemBlueprint.ItemDescription;
        }

        #if UNITY_EDITOR

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            ItemRuntimeId = -1;
            ItemAssetId = ItemAssetIdName.ComputeFNV1aHash();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        #endif
    }
}