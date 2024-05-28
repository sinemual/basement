using System;

namespace Client.Infrastructure
{
    public class SoftCurrencyEvents
    {
        public event Action GetSoftCurrency;
        public event Action SpendSoftCurrency;
        
        public void OnGetSoftCurrency() => GetSoftCurrency?.Invoke();
        public void OnSpendSoftCurrency() => SpendSoftCurrency?.Invoke();
    }
}