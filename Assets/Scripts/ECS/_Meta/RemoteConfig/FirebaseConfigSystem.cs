using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;
#if mamboo_firebase
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif

namespace Client
{
    public class FirebaseConfigSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private FirebaseRemoteConfigService _firebaseRemoteConfigService;
        private AnalyticService _analyticService;

        private EcsFilter<FirebaseConfigFetchRequest> _requestFilter;
        private EcsFilter<CheckFirebaseLoadProcess>.Exclude<InitedMarker> _initFilter;

        public void Init()
        {
            EcsEntity checkLoadEntity = _world.NewEntity();
            checkLoadEntity.Get<CheckFirebaseLoadProcess>();
        }

        public void Run()
        {
#if remote_congig_firebase
            if (!_firebaseRemoteConfigService.IsConected || !_firebaseRemoteConfigService.IsFetched)
                return;

            foreach (var idx in _initFilter)
            {
                ref var entity = ref _initFilter.GetEntity(idx);

                SetDefaultValues();
                InitRemoteConfigData();
                entity.Get<InitedMarker>();
            }

            foreach (var request in _requestFilter)
            {
                ref var entity = ref _requestFilter.GetEntity(request);
                _initFilter.GetEntity(0).Del<InitedMarker>();
                _firebaseRemoteConfigService.ReFetch();
                entity.Del<FirebaseConfigFetchRequest>();
            }
#endif
        }

        private void InitRemoteConfigData()
        {
            _world.NewEntity().Get<FirebaseRemoteConfigLastFetchSuccessEvent>();
            LoadSettings();
            RunAbTests();
            Segmentation();
            SettingForAds();
        }

        private void SetDefaultValues()
        {
#if remote_congig_firebase
            Dictionary<string, object> defaults = new Dictionary<string, object>();

            defaults.Add("segment_group_name", "default");
            //defaults.Add("magic_reward", 1000);
            //defaults.Add("force_redirect_player_to_store", false);
            defaults.Add("InterstitialSettingsData", _data.InterstitialSettingsData);
            defaults.Add("RewardEquipUpgradeData", _data.RewardEquipUpgradeData);
            defaults.Add("RateUsData", _data.RateUsData);
            //defaults.Add("BalanceData", _data.BalanceData);

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
#endif
        }

        private void Segmentation()
        {
#if remote_congig_firebase
            if (FirebaseRemoteConfig.DefaultInstance.GetValue("segment_group_name").StringValue != "")
            {
                string groupName = FirebaseRemoteConfig.DefaultInstance.GetValue("segment_group_name").StringValue;
                _analyticService.LogEvent("segment_group_name_" + groupName);
            }
#endif
        }

        private void SettingForAds()
        {
#if remote_congig_firebase
            Debug.Log("Firebase remote configs:");
            Debug.Log(
                $"[REMOTE CONFIG] InterstitialSettingsData: {FirebaseRemoteConfig.DefaultInstance.GetValue("InterstitialSettingsData").StringValue}");
            Debug.Log(
                $"[REMOTE CONFIG] RewardEquipUpgradeData {FirebaseRemoteConfig.DefaultInstance.GetValue("RewardEquipUpgradeData").StringValue}");
            Debug.Log(
                $"[REMOTE CONFIG] RateUsData {FirebaseRemoteConfig.DefaultInstance.GetValue("RateUsData").StringValue}");

            if (FirebaseRemoteConfig.DefaultInstance.GetValue("InterstitialSettingsData").StringValue != "")
                JsonUtility.FromJsonOverwrite(FirebaseRemoteConfig.DefaultInstance.GetValue("InterstitialSettingsData").StringValue,
                    _data.InterstitialSettingsData);

            if (FirebaseRemoteConfig.DefaultInstance.GetValue("RewardEquipUpgradeData").StringValue != "")
                JsonUtility.FromJsonOverwrite(FirebaseRemoteConfig.DefaultInstance.GetValue("RewardEquipUpgradeData").StringValue,
                    _data.RewardEquipUpgradeData);
            
            if (FirebaseRemoteConfig.DefaultInstance.GetValue("RateUsData").StringValue != "")
                JsonUtility.FromJsonOverwrite(FirebaseRemoteConfig.DefaultInstance.GetValue("RateUsData").StringValue,
                    _data.RateUsData);
#endif
        }

        private void LoadSettings()
        {
            /*if (FirebaseRemoteConfig.DefaultInstance.GetValue("BalanceData").StringValue != "")
                JsonUtility.FromJsonOverwrite(FirebaseRemoteConfig.DefaultInstance.GetValue("BalanceData").StringValue,
                    _gameData.BalanceData);*/

            //_data.RuntimeData.UiColorNum = (int)FirebaseRemoteConfig.DefaultInstance.GetValue("ui_color").LongValue;
            //_analyticService.LogEventWithParameter("ab_ui_color", "ui_color", _data.RuntimeData.UiColorNum);
        }

        private void RunAbTests()
        {
            //_data.SceneData.RunUiShopPanelAbTest(_data.SceneData.AbTestData.uiShopPanelTestAbs, _data.RuntimeData.ShopPanelNum);
        }
    }


    internal struct FirebaseRemoteConfigLastFetchSuccessEvent : IEcsIgnoreInFilter
    {
    }

    public struct CheckFirebaseLoadProcess : IEcsIgnoreInFilter
    {
    }

    internal struct FirebaseConfigFetchRequest : IEcsIgnoreInFilter
    {
    }
}