using UnityEngine;

namespace GameCore.Examples
{
    public class SkinUnlockable : BaseUnlockable
    {
        [SerializeField] private Currency _currency;
        [SerializeField] private int _cost;
        [SerializeField] private string _requiredAchievement;
        
        public Currency CurrencyType => _currency;
        public int Cost => _cost;

        public override bool CanUnlock(IPlayerUnlockProgress unlockProgress)
        {
            return unlockProgress.GetCurrencyBalance(_currency) >= _cost &&
                   (string.IsNullOrEmpty(_requiredAchievement) ||
                    unlockProgress.HasAchievement(_requiredAchievement));
        }

        public override void OnUnlocked()
        {
            // Deduct currency logic here
        }
    }
}