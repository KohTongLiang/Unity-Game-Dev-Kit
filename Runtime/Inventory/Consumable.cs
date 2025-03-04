using System.Collections.Generic;

namespace GameCore
{
    public class Consumable
    {
        private readonly Dictionary<int, int> consumableLookup = new();
        private readonly List<Stack<IConsumable>> consumableSlots = new();

        /// <summary>
        /// Add consumable into available slot, or create new slot for it.
        /// </summary>
        /// <param name="consumable"></param>
        public void AddConsumable(IConsumable consumable, int assetId)
        {
            if (consumableLookup.TryGetValue(assetId, out var slotIdx))
            {
                consumableSlots[slotIdx].Push(consumable);
            }
            else
            {
                var newSlot = new Stack<IConsumable>();
                newSlot.Push(consumable);
                consumableSlots.Add(newSlot);
                consumableLookup.Add(assetId, consumableSlots.Count - 1);
            }
        }

        /// <summary>
        /// Attempts to use a consumable by passing in a slot index, and retrieving a consumable.
        /// </summary>
        /// <param name="slotIdx">Index of the consumable slot to use</param>
        /// <param name="removeWhenEmpty">Whether to clear the slot after all consumables are used up.</param>
        /// <returns></returns>
        public bool TryUseConsumable(int slotIdx, bool removeWhenEmpty = false)
        {
            if (consumableSlots.Count <= 0 || consumableSlots.Count - 1 < slotIdx) return false;
            if (consumableSlots[slotIdx].TryPop(out var consumable))
            {
                consumable.Consume();
            }

            // Depending on game, we may or may not want to clear the consumables.
            if (removeWhenEmpty && consumableSlots[slotIdx].Count <= 0)
            {
                var item = (Item)consumable;
                consumableLookup.Remove(item.ItemAssetId);
                consumableSlots.RemoveAt(slotIdx);
            }

            return true;
        }
    }
}