using System;
using System.Collections.Generic;
using UnityEngine;
using UnityServiceLocator;

namespace GameCore
{
    public class CurrencyManager : MonoBehaviour
    {
        [SerializeField] private List<Currency> _currencies;
        
        private readonly Dictionary<Currency, int> _currencyBalances = new();

        private void Awake()
        {
            ServiceLocator.Global.Register(this);
        }

        public void Initialize()
        {
            foreach (var currency in _currencies)
            {
                _currencyBalances[currency] = currency.defaultAmount;
            }
        }

        public int GetBalance(Currency currency)
        {
            return _currencyBalances.TryGetValue(currency, out var balance) ? balance : 0;
        }

        public void AddCurrency(Currency currency, int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"Cannot add negative amount for {currency.currencyName}");
                return;
            }

            if (!_currencyBalances.ContainsKey(currency))
            {
                _currencyBalances[currency] = currency.defaultAmount;
            }

            _currencyBalances[currency] = Mathf.Min(_currencyBalances[currency] + amount, currency.maxAmount);
            OnCurrencyChanged?.Invoke(currency, _currencyBalances[currency]);
        }

        public bool SpendCurrency(Currency currency, int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"Cannot spend negative amount for {currency.currencyName}");
                return false;
            }

            if (!_currencyBalances.ContainsKey(currency) || _currencyBalances[currency] < amount)
            {
                Debug.LogWarning($"Not enough {currency.currencyName} to spend");
                return false;
            }

            _currencyBalances[currency] -= amount;
            OnCurrencyChanged?.Invoke(currency, _currencyBalances[currency]);
            return true;
        }

        // Event for UI updates
        public event Action<Currency, int> OnCurrencyChanged; 
    }
}