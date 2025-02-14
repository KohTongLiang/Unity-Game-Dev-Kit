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

        // The lookup table for the individual items
        private Dictionary<int, Item> itemList = new();

        // Lookup table organised by the asset id of the items. Each entry conatains a set
        // of unique item Ids for each asset id. An asset Id could be like "Potion", and each id
        // could represent an instance of the poition.
        private Dictionary<int, HashSet<int>> itemIdsByAssetId = new();


        public void ReceiveItem(Item item)
        {
            if (itemList.TryGetValue(item.ItemRuntimeId, out var itemExist) && itemExist) return;
            itemList.Add(item.ItemRuntimeId, item);

            if (itemIdsByAssetId.TryGetValue(item.ItemAssetId, out var ids))
            {
                ids.Add(item.ItemAssetId);
            }
            else
            {
                itemIdsByAssetId.Add(item.ItemAssetId, new HashSet<int> { item.ItemAssetId });
            }

            OnReceiveItem?.Invoke(item);
        }

        public Item RemoveItemById(int runtimeId)
        {
            var item = itemList[runtimeId];
            itemList.Remove(runtimeId);
            if (itemIdsByAssetId.TryGetValue(item.ItemAssetId, out var ids))
            {
                ids.Remove(item.ItemRuntimeId);
                if (ids.Count == 0) itemIdsByAssetId.Remove(item.ItemAssetId);
            }
            OnRemoveItem?.Invoke(item);
            return item;
        }

        /// <summary>
        /// Attempts to retrieve an item from the inventory.
        /// </summary>
        /// <param name="itemId">The runtime id of the item.</param>
        /// <param name="item">Returns a Item.</param>
        /// <returns></returns>
        public bool TryGetItemById (int itemId, out Item item)
        {
            return itemList.TryGetValue(itemId, out item);
        }

        /// <summary>
        /// Attempts to retrieve a HashSet of item ids from the inventory.
        /// </summary>
        /// <param name="assetId">Asset Id.</param>
        /// <param name="itemIds">Returns a HashSet<int> with the item ids inside</param>
        /// <returns></returns>
        public bool TryGetItemsByAssetId(int assetId, out HashSet<int> itemIds)
        {
            return itemIdsByAssetId.TryGetValue(assetId, out itemIds);
        }

        /// <summary>
        /// Get the quantity of an item in player inventory.
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public int GetQuantityOfAssetItem(int assetId)
        {
            if (!itemIdsByAssetId.ContainsKey(assetId))
            {
                Debug.LogWarning($"Asset Id {assetId} not found");
                return 0;
            }
            return itemIdsByAssetId[assetId].Count;
        }

        public bool CheckItemExist(int assetId)
        {
            return itemList.ContainsKey(assetId);
        }

        /// <summary>
        /// Purge inventory system
        /// </summary>
        public void Clear()
        {
            foreach (var inventoryDatastoreValue in itemList.Values)
            {
                OnRemoveItem?.Invoke(inventoryDatastoreValue);
            }

            itemList.Clear();
        }
    }
}