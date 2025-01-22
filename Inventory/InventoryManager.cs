using System;
using System.Collections.Generic;

namespace GameCore
{
    public class InventoryManager
    {
        private readonly List<Item> _tempItemList = new();
        public List<Item> TempItemList => _tempItemList;
        public Action<Item> OnReceiveItemCallback { get; set; }
        public Action<Item> OnRemoveItemCallback { get; set; }

        public List<Item> GetItemList()
        {
            return _tempItemList;
        }

        public void ReceiveItem(Item item)
        {
            _tempItemList.Add(item);
            OnReceiveItemCallback?.Invoke(item);
        }

        public Item RemoveItemByAssetId(string assetId)
        {
            var item = _tempItemList.Find(item => item.itemAssetId == assetId);
            _tempItemList.Remove(item);
            OnRemoveItemCallback?.Invoke(item);
            return item;
        }

        public bool CheckItemExist(string assetIds)
        {
            return _tempItemList.Exists(item => item.itemAssetId == assetIds);
        }

        public void Clear()
        {
            _tempItemList.Clear();
        }
    }
}