using System;
using Client;
using Client.Data.Core;
using Leopotam.Ecs;

public class AdsService
{
    private EcsEntity _timerEntity;
    private SharedData _data;
    private AnalyticService _analyticService;
    private TimeManagerService _timeManagerService;

    public AdsService(EcsWorld ecsWorld, SharedData data, AnalyticService analyticService, TimeManagerService timeManagerService)
    {
        _data = data;
        _analyticService = analyticService;
        _timeManagerService = timeManagerService;
        
        _timerEntity = ecsWorld.NewEntity();
        _timerEntity.Get<InterBusTimer>();
        _timerEntity.Get<Timer<InterReloadTimer>>().Value = 20.0f;
        
        CheckInterResetCapTimer();
        if (_timeManagerService.IsNetTimeMoreThanSavedValue(ref _data.PlayerData.InterCapRestartTimeKey))
            _timeManagerService.SaveWithNetTime(ref _data.PlayerData.InterCapRestartTimeKey, _data.BalanceData.ResetInterDailyCapTime);
    }

    public void ShowInter(string placement)
    {
        if (_data.PlayerData.IsNoAdsIapBuyed)
            return;
        
        if (!_timerEntity.Has<Timer<InterReloadTimer>>() &&
            _data.PlayerData.InterstitalCounter < _data.InterstitialSettingsData.DailyCap)
        {
            //AdsManager.Instance.ShowInterstitial(placement);
            _analyticService.LogAdsEvent(AdsType.Interstitial, placement);
            _timerEntity.Get<Timer<InterReloadTimer>>().Value = _data.InterstitialSettingsData.Interval;
            _data.PlayerData.InterstitalCounter += 1;
        }
    }

    public void ShowRewardVideo(string placement, Action getRewardMethod)
    {
        /*AdsManager.RewardAction = () =>
        {
            getRewardMethod.Invoke();
            _timerEntity.Get<Timer<InterReloadTimer>>().Value = _data.InterstitialSettingsData.Interval;
            _analyticService.LogEventWithParameter("get_reward", placement);
        };
        AdsManager.Instance.ShowReward(placement);*/
        _analyticService.LogAdsEvent(AdsType.Reward, placement);
    }

    public bool IsRewardVideoReady()
    {
#if UNITY_EDITOR
        return true;
#else
        return AdsManager.Instance.adsService.HasReward();
#endif
    }

    public void ShowBanner(string placement)
    {
        if (_data.PlayerData.IsNoAdsIapBuyed)
            return;
        
        //AdsManager.Instance.ShowBanner(placement);
        _analyticService.LogAdsEvent(AdsType.Banner, placement);
    }

    private void CheckInterResetCapTimer()
    {
        if (_data.PlayerData.InterstitalCounter >= _data.InterstitialSettingsData.DailyCap)
            if (_timeManagerService.IsNetTimeMoreThanSavedValue(ref _data.PlayerData.InterCapRestartTimeKey))
                _data.PlayerData.InterstitalCounter = 0;
    }

    public enum AdsType
    {
        Banner = 0,
        Interstitial = 1,
        Reward = 2,
    }
}

public struct InterBusTimer
{
}