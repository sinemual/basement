using System;

namespace Client.Infrastructure
{
    public class NoAdsIapScreenEvents
    {
        public event Action BuyNoAdsIapButtonTap;
        
        public void OnBuyNoAdsIapButtonTap() => BuyNoAdsIapButtonTap?.Invoke();
    }
}