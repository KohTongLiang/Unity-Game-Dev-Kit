using UnityEngine;

namespace GameCore.Examples
{
    public class LevelUnlockable : BaseUnlockable
    {
        [SerializeField] private int _requiredLevel;

        public override bool CanUnlock(IPlayerUnlockProgress unlockProgress)
        {
            return unlockProgress.GetProgressionLevel() >= _requiredLevel;
        }
    }
}