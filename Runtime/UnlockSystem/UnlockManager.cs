using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    public class UnlockManager : MonoBehaviour
    {
        [SerializeField] private List<BaseUnlockable> unlockables;
        public List<BaseUnlockable> Unlockables => unlockables;
        
        private void Awake()
        {
            ServiceLocator.Global.Register(this);
        }

        #region Core System
        
        private readonly Dictionary<string, IUnlockable> _unlockables = new Dictionary<string, IUnlockable>();
        private IPlayerUnlockProgress _unlockProgress;
        private ISaveSystem _saveSystem;
    
        public void Initialize(IPlayerUnlockProgress unlockProgress, ISaveSystem saveSystem)
        {
            unlockables.ForEach(RegisterUnlockable);
            _unlockProgress = unlockProgress;
            _saveSystem = saveSystem;
            SaveUnlockables(); // Save unlocked by default items
            LoadUnlockables();
        }

        public void RegisterUnlockable(IUnlockable unlockable)
        {
            if (!_unlockables.ContainsKey(unlockable.ID))
            {
                _unlockables.Add(unlockable.ID, unlockable);
            }
        }

        public bool TryUnlock(string unlockableId)
        {
            if (!_unlockables.TryGetValue(unlockableId, out var unlockable)) return false;
            if (unlockable.IsUnlocked || !unlockable.CanUnlock(_unlockProgress)) return false;

            unlockable.IsUnlocked = true;
            unlockable.OnUnlocked();
            SaveUnlockables();
            return true;
        }
        
        #endregion

        #region Save/Load
        
        private const string UNLOCK_DATA_KEY = "Unlockables";
    
        private void SaveUnlockables()
        {
            var saveData = new List<UnlockableData>();
            foreach (var unlockable in _unlockables.Values)
            {
                if (!unlockable.IsUnlocked) continue;
                saveData.Add(new UnlockableData {
                    id = unlockable.ID,
                    isUnlocked = unlockable.IsUnlocked
                });
            }
            
            Debug.Log($"{JsonConvert.SerializeObject(saveData)}");
            _saveSystem.Save(UNLOCK_DATA_KEY, saveData, MergePolicy.Overwrite);
        }

        private void LoadUnlockables()
        {
            if (!_saveSystem.SaveExists(UNLOCK_DATA_KEY)) return;
        
            var loadedData = _saveSystem.Load<List<UnlockableData>>(UNLOCK_DATA_KEY);
            foreach (var data in loadedData)
            {
                if (_unlockables.TryGetValue(data.id, out var unlockable))
                {
                    unlockable.IsUnlocked = data.isUnlocked;
                }
            }
        }
        #endregion 
    }
}