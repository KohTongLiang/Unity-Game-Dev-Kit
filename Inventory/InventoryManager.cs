using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class InventoryManager
    {
        public delegate void ItemReceivedEvent(Item item);
        public delegate void ItemRemovedEvent(Item item);

        public event ItemReceivedEvent OnReceiveItem;
        public event ItemRemovedEvent OnRemoveItem;


        // Lookup table organised by the asset id of the items. Each entry conatains a set
        // of unique item Ids for each asset id. An asset Id could be like "Potion", and each item
        // represents an instance of the "Potion".
        private Dictionary<int, Dictionary<int, Item>> itemDictionary = new();

        public void ReceiveItem(Item item)
        {
            if (itemDictionary.TryGetValue(item.ItemAssetId, out var ids))
            {
                // Update existing dictionary
                ids.Add(item.ItemRuntimeId, item);
                itemDictionary[item.ItemAssetId] = ids;
            }
            else
            {
                // Create new dictionary entry
                itemDictionary.Add(item.ItemAssetId, new Dictionary<int, Item> { { item.ItemRuntimeId, item } });
            }

            OnReceiveItem?.Invoke(item);
        }

        public void RemoveItem(Item item)
        {
            if (itemDictionary.TryGetValue(item.ItemAssetId, out var ids))
            {
                ids.Remove(item.ItemRuntimeId);
                if (ids.Count == 0) itemDictionary.Remove(item.ItemAssetId);
                else itemDictionary[item.ItemAssetId] = ids;
            }
            else
            {
                return;
            }

            OnRemoveItem?.Invoke(item);
        }

        /// <summary>
        /// Attempts to retrieve a HashSet of item ids from the inventory.
        /// </summary>
        /// <param name="assetId">Asset Id.</param>
        /// <param name="items">Pass by reference the lookup for the items.</param>
        /// <returns></returns>
        public bool TryGetItemsByAssetId(int assetId, ref Dictionary<int, Item> items)
        {
            return itemDictionary.TryGetValue(assetId, out items);
        }

        /// <summary>
        /// Get the quantity of an item in player inventory.
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public int GetQuantityOfAssetItem(int assetId)
        {
            if (!itemDictionary.TryGetValue(assetId, value: out var value))
            {
                Debug.LogWarning($"Asset Id {assetId} not found");
                return 0;
            }
            return value.Count;
        }

        /// <summary>
        /// Check if item is already in the Inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckItemExist(Item item)
        {
            return itemDictionary.TryGetValue(item.ItemAssetId, out var value) && value.ContainsKey(item.ItemRuntimeId);
        }

        /// <summary>
        /// Query item from Inventory by runtime id.
        /// </summary>
        /// <param name="assetId">Item's asset id to retrieve the correcy lookup.</param>
        /// <param name="runtimeId">Item's runtime id.</param>
        /// <param name="item">Output item</param>
        public void GetItemByRuntimeId(int assetId, int runtimeId, out Item item)
        {
            if (itemDictionary.TryGetValue(assetId, out var value) && value.TryGetValue(runtimeId, out item))
            {
                return;
            }
            item = null;
        }

        /// <summary>
        /// Purge inventory.
        /// </summary>
        public void Clear()
        {
            foreach (var inventoryDatastoreValue in itemDictionary.Values)
            {
                foreach (var item in inventoryDatastoreValue.Values)
                {
                    OnRemoveItem?.Invoke(item);
                }
                inventoryDatastoreValue.Clear();
            }

            itemDictionary.Clear();
        }

        public List<T> RetrieveAllItems<T>() where T : Item
        {
            var items = new List<T>();
            foreach (var inventoryDatastoreValue in itemDictionary.Values)
            {
                foreach (var item in inventoryDatastoreValue.Values)
                {
                    items.Add(item as T);
                }
            }
            return items;
        }

        public int GetItemCount()
        {
            int count = 0;
            foreach (var itemDict in itemDictionary.Values)
            {
                count += itemDict.Count;
            }

            return count;
        }
    }
}