using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Tab cycle based equipment bar
    /// </summary>
    public class Equipment
    {
        private readonly List<IEquippable> _equippables = new();
        private int _currentEquippableIndex;

        // Whether equipments are equipped.
        private bool equipped = false;

        public void AddEquippable(IEquippable equippable)
        {
            _equippables.Add(equippable);
        }

        public void RemoveEquippable(IEquippable equippable)
        {
            var idx = _equippables.FindIndex(e => e == equippable);
            if (_currentEquippableIndex == idx)
            {
                equippable.UnEquip();
            }

            _equippables.Remove(equippable);

            if (_equippables.Count == 0)
            {
                equipped = false;
                _currentEquippableIndex = 0;
            }
            else if (_currentEquippableIndex >= _equippables.Count)
            {
                _currentEquippableIndex = _equippables.Count - 1;
            }
        }

        /// <summary>
        /// Cycle equipment left, if at the start of the list, go to the last equipment.
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
        /// Cycles equipment right, if at the end of the list, go back to the first equipment.
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
        /// Directly equips an equipment by index.
        /// </summary>
        /// <param name="index"></param>
        public void DirectEquip(int index)
        {
            if (index >= _equippables.Count) return;
            UnEquipCurrent();
            _currentEquippableIndex = index;
            EquipCurrent();
            equipped = true;
        }

        private void UnEquipCurrent()
        {
            if (!equipped || _equippables.Count <= 0) return;
            _equippables[_currentEquippableIndex].UnEquip();
            equipped = false;
        }

        private void EquipCurrent()
        {
            if (equipped || _equippables.Count <= 0) return;
            _equippables[_currentEquippableIndex].Equip();
            equipped = true;
        }

        public void UseCurrent()
        {
            if (!equipped) return;
            _equippables[_currentEquippableIndex].Use();
        }

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

        public List<IEquippable> GetAllEquipments() => _equippables;

        public void Clear()
        {
            equipped = false;
            _currentEquippableIndex = 0;
            _equippables.Clear();
        }
    }
}