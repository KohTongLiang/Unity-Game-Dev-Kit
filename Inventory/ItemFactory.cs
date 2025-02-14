using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ItemFactory : Singleton<ItemFactory>
    {
        private readonly Dictionary<int, int> itemTracker = new();

        /// <summary>
        /// Asks the factory to instantiate a new instance of an Item
        /// </summary>
        /// <param name="itemBlueprint"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Item CreateItem(ItemSo itemBlueprint, Vector3 position)
        {
            int generatedId;
            if (itemTracker.TryGetValue(itemBlueprint.ItemAssetId, out var count))
            {
                generatedId = count++;
            }
            else
            {
                generatedId = 0;
                itemTracker.Add(itemBlueprint.ItemAssetId, 1);
            }

            var item = Instantiate(itemBlueprint.ItemPrefab).GetComponent<Item>();
            item.ItemRuntimeId = generatedId;
            item.ItemAssetId = itemBlueprint.ItemAssetId;
            item.ItemName = itemBlueprint.ItemName;
            item.ItemDescription = itemBlueprint.ItemDescription;
            item.transform.position = position;
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
        }
    }
}