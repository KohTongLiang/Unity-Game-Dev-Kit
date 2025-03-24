using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "NewCurrency", menuName = "Unlockables/Currency", order = 0)]
    public class Currency : ScriptableObject
    {
        public string currencyName; // e.g., "Coins", "Gems"
        public Sprite icon; // For UI display
        public int defaultAmount = 0; // Starting amount
        public bool isPremium = false; // Is this a premium (real-money) currency?

        // Optional: Add more metadata like description, max limit, etc.
        [TextArea] public string description;
        public int maxAmount = 999999; // Max limit for this currency 
    }
}