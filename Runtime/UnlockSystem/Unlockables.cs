using System;
using UnityEngine;

namespace GameCore
{
    public interface IUnlockable
    {
        string ID { get; }
        string DisplayName { get; }
        bool IsUnlocked { get; set; }
        Sprite PreviewImage { get; }
        bool CanUnlock(IPlayerUnlockProgress unlockProgress);
        void OnUnlocked();
    }

    /// <summary>
    /// Progress of player's unlock in the game
    /// </summary>
    public interface IPlayerUnlockProgress
    {
        int GetCurrencyBalance(Currency currency);
        int GetProgressionLevel();
        bool HasAchievement(string achievementId);
    }

    [Serializable]
    public class UnlockableData
    {
        public string id;
        public bool isUnlocked;
    }
    
    [Serializable]
    // Example implementations
    public abstract class BaseUnlockable : MonoBehaviour, IUnlockable
    {
        [field: SerializeField]
        public string ID { get; protected set; }
        
        [field: SerializeField]
        public string DisplayName { get; protected set; }
        
        [field: SerializeField]
        public bool IsUnlocked { get; set; }
        
        [field: SerializeField]
        public Sprite PreviewImage { get; protected set; }
    
        public abstract bool CanUnlock(IPlayerUnlockProgress unlockProgress);
        public virtual void OnUnlocked() { }
    }
}