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
        [SerializeField] private bool registerOnAwake = false;
        public string ItemAssetIdName;
        public string ItemName;
        public string ItemDescription;

        protected virtual void Awake()
        {
            if (registerOnAwake)
            {
                ItemFactory.Instance.RegisterItem(this);
            }
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