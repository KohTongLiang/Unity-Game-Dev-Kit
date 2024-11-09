using System;
using UnityEngine;

namespace GameCore
{
    public class Item : MonoBehaviour
    {
        public Guid itemId = new();
        public string itemAssetId; // id in editor mode, at runtime a ulong is generated
        public string itemName;
        public string itemDescription;
    }
}