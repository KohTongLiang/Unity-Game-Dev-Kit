using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Tab cycle based equipment bar for managing and equipping items.
    /// </summary>
    public class Equipment
    {
        private readonly List<IEquippable> _equippables = new();
        private int _currentEquippableIndex;

        // Tracks whether equipment is currently equipped.
        private bool equipped = false;

        /// <summary>
        /// Indicates if any equipment is currently equipped.
        /// </summary>
        public bool Equipped => equipped;

        // Determines behavior when removing equipment: either push the list forward or set a default equipment.
        private bool pushEquipmentForward;

        /// <summary>
        /// Default equipment to equip when necessary.
        /// </summary>
        public IEquippable DefaultEquipment;

        private int maxLimit;

        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment"/> class.
        /// </summary>
        /// <param name="pushEquipmentForward">If true, equipment list advances when one is removed.</param>
        /// <param name="maxLimit">Maximum number of equippable items allowed.</param>
        public Equipment(bool pushEquipmentForward, int maxLimit)
        {
            this.pushEquipmentForward = pushEquipmentForward;
            this.maxLimit = maxLimit;
        }

        /// <summary>
        /// Adds an equippable item to the equipment list.
        /// </summary>
        /// <param name="equippable">The equippable item to add.</param>
        public void AddEquippable(IEquippable equippable)
        {
            if (_equippables.Count >= maxLimit)
            {
                int defaultIndex = _equippables.FindIndex(e => e == DefaultEquipment);
                if (defaultIndex != -1)
                {
                    _equippables[defaultIndex] = equippable;
                }
                return;
            }

            int defaultIdx = _equippables.FindIndex(e => e == DefaultEquipment);
            if (defaultIdx != -1)
            {
                _equippables.Insert(defaultIdx, equippable);
            }
            else
            {
                _equippables.Add(equippable);
            }
        }

        /// <summary>
        /// Removes an equippable item from the equipment list.
        /// </summary>
        /// <param name="equippable">The equippable item to remove.</param>
        public void RemoveEquippable(IEquippable equippable)
        {
            var idx = _equippables.FindIndex(e => e == equippable);
            if (_currentEquippableIndex == idx)
            {
                equippable.UnEquip();
            }

            _equippables.RemoveAt(idx);

            if (pushEquipmentForward)
            {
                _currentEquippableIndex = _equippables.Count > 0 ? Mathf.Clamp(_currentEquippableIndex, 0, _equippables.Count - 1) : 0;
            }
            else
            {
                _currentEquippableIndex = idx >= _equippables.Count ? _equippables.Count - 1 : idx;
                if (DefaultEquipment != null)
                {
                    _equippables.Insert(_currentEquippableIndex, DefaultEquipment);
                }
            }

            if (equipped) equipped = false;
            if (_equippables.Count == 0)
            {
                _currentEquippableIndex = 0;
            }
            else if (_currentEquippableIndex >= _equippables.Count)
            {
                _currentEquippableIndex = _equippables.Count - 1;
            }

            DirectEquip(_currentEquippableIndex);
        }

        /// <summary>
        /// Cycles equipment to the left. Wraps around to the last equipment if at the start.
        /// </summary>
        public void CycleEquipmentLeft()
        {
            if (equipped)
            {
                UnEquipCurrent();
                _currentEquippableIndex = Mathf.Abs(_currentEquippableIndex - 1) % _equippables.Count;
                EquipCurrent();
                return;
            }

            _currentEquippableIndex = Mathf.Abs(_currentEquippableIndex - 1) % _equippables.Count;
        }

        /// <summary>
        /// Cycles equipment to the right. Wraps around to the first equipment if at the end.
        /// </summary>
        public void CycleEquipmentRight()
        {
            if (equipped)
            {
                UnEquipCurrent();
                _currentEquippableIndex = (_currentEquippableIndex + 1) % _equippables.Count;
                EquipCurrent();
                return;
            }

            _currentEquippableIndex = (_currentEquippableIndex + 1) % _equippables.Count;
        }

        /// <summary>
        /// Directly equips an equipment by its index.
        /// </summary>
        /// <param name="index">The index of the equipment to equip.</param>
        /// <returns>true if equip successful; otherwise, false.</returns>
        public bool DirectEquip(int index)
        {
            if (index >= _equippables.Count) return false;
            UnEquipCurrent();
            _currentEquippableIndex = index;
            EquipCurrent();
            equipped = true;
            return true;
        }

        /// <summary>
        /// Unequips the currently equipped item.
        /// </summary>
        public void UnEquipCurrent()
        {
            if (!equipped || _equippables.Count <= 0) return;
            if (_currentEquippableIndex >= _equippables.Count) return;
            _equippables[_currentEquippableIndex].UnEquip();
            equipped = false;
        }

        /// <summary>
        /// Equips the current item based on the current index.
        /// </summary>
        private void EquipCurrent()
        {
            if (equipped || _equippables.Count <= 0) return;
            if (_currentEquippableIndex >= _equippables.Count) return;
            _equippables[_currentEquippableIndex].Equip();
            equipped = true;
        }

        /// <summary>
        /// Uses the currently equipped item.
        /// </summary>
        public void UseCurrent()
        {
            if (!equipped || _equippables.Count <= 0) return;
            if (_currentEquippableIndex >= _equippables.Count) return;
            _equippables[_currentEquippableIndex].Use();
        }

        /// <summary>
        /// Toggles the state of the current equipment between equipped and unequipped.
        /// </summary>
        public void ToggleEquipment()
        {
            if (equipped)
            {
                UnEquipCurrent();
            }
            else
            {
                EquipCurrent();
            }
        }

        /// <summary>
        /// Retrieves all equippable items.
        /// </summary>
        /// <returns>A list of all equippable items.</returns>
        public List<IEquippable> GetAllEquipments() => _equippables;

        /// <summary>
        /// Clears all equipment from the list and resets the current index.
        /// </summary>
        public void Clear()
        {
            equipped = false;
            _currentEquippableIndex = 0;
            _equippables.Clear();
        }
    }
}