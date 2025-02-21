using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ItemFactory : Singleton<ItemFactory>
    {
        private readonly Dictionary<int, int> itemTracker = new();
        private readonly Dictionary<int, Dictionary<int, GameObject>> itemDictionary = new();

        /// <summary>
        /// Asks the factory to instantiate a new instance of an Item
        /// </summary>
        /// <param name="itemBlueprint"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public T CreateItem<T>(ItemSo itemBlueprint, Vector3 position) where T : Item
        {
            int generatedId;
            if (itemTracker.TryGetValue(itemBlueprint.ItemAssetId, out var count))
            {
                generatedId = count++;
                itemTracker[itemBlueprint.ItemAssetId] = count;
            }
            else
            {
                generatedId = 0;
                itemTracker.Add(itemBlueprint.ItemAssetId, 1);
            }

            var item = Instantiate(itemBlueprint.ItemPrefab).GetComponent<T>();
            item.ItemRuntimeId = generatedId;
            item.ItemAssetId = itemBlueprint.ItemAssetId;
            item.ItemName = itemBlueprint.ItemName;
            item.ItemDescription = itemBlueprint.ItemDescription;
            item.transform.position = position;

            if (itemDictionary.TryGetValue(item.ItemAssetId, out var items))
            {
                items.Add(item.ItemRuntimeId, item.gameObject);
            }
            else
            {
                itemDictionary[item.ItemAssetId] = new Dictionary<int, GameObject> { { item.ItemRuntimeId, item.gameObject } };
            }

            return item;
        }

        /// <summary>
        /// Register already instantiated item with the factory. End result is the item receives a unique id.
        /// </summary>
        public void RegisterItem(Item item)
        {
            if (itemTracker.TryGetValue(item.ItemAssetId, out var count))
            {
                item.ItemRuntimeId = count;
                itemTracker[item.ItemAssetId] = count + 1;
            }
            else
            {
                item.ItemRuntimeId = 0;
                itemTracker.Add(item.ItemAssetId, 1);
            }

            if (itemDictionary.TryGetValue(item.ItemAssetId, out var items))
            {
                items.Add(item.ItemRuntimeId, item.gameObject);
            }
            else
            {
                itemDictionary[item.ItemAssetId] = new Dictionary<int, GameObject> { { item.ItemRuntimeId, item.gameObject } };
            }
        }

        public GameObject RetrieveItemObject(Item item)
        {
            if (!itemDictionary.TryGetValue(item.ItemAssetId, out var value))
            {
                return null;
            }

            return value.GetValueOrDefault(item.ItemRuntimeId);
        }

        public void ClearAllItems()
        {
            foreach (var itemDict in itemDictionary.Values)
            {
                foreach (var itemObj in itemDict.Values) Destroy(itemObj.gameObject);
                itemDict.Clear();
            }

            itemDictionary.Clear();
            itemTracker.Clear();
        }
    }
}