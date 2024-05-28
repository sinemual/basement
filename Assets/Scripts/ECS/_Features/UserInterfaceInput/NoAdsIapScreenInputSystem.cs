using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client
{
    public class NoAdsIapScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _userInterfaceEventBus;
        private AnalyticService _analyticService;
        private VibrationService _vibrationService;
        private AudioService _audioService;
        private AdsService _adsService;
        
        public void Init()
        {
            _userInterfaceEventBus.NoAdsIapScreen.BuyNoAdsIapButtonTap += () =>
            {
                /*PurchaseController.instance.PurchaseProduct("no_ads");
                PurchaseController.instance.OnPurchaseCompleted = x =>
                {
                    if (x == "no_ads")
                    {
                        _data.PlayerData.IsNoAdsIapBuyed = true;
                        _analyticService.LogEvent("no_ads_iap_buyed");
                    }
                };*/
                
                _analyticService.LogEvent("buy_no_ads_iap_tap");
                
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.UiClickSound);
            };
        }
    }
}