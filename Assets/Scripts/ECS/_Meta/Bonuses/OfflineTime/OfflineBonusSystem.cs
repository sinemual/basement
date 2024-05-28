using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class OfflineBonusSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private EcsFilter<GetOfflineBonusRewardRequest> _filter;

        private double reward = 0;

        public void Init()
        {
            //var sec = 0; (int)Mathf.Clamp(TimeManagerService.Offline.LoadOfflineBonus().Second, 60 * 60,60 * 60 * _gameData.BalanceData.OfflineTimeHourCap.y);
            //reward = sec * _gameData.PlayerData.MoneyInSec;
            /*if (sec > 60 * 60 * _gameData.BalanceData.OfflineTimeHourCap.x)
                _ui.SetShowStateOfflineBonusScreen(true, reward);*/
        }

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var rewardRequest = ref entity.Get<GetOfflineBonusRewardRequest>();

                _world.NewEntity().Get<AddCurrencyRequest>().Value = reward;
                entity.Del<GetOfflineBonusRewardRequest>();
            }
        }
    }
}