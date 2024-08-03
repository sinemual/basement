using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using Data;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client
{
    public class ExplosionCharacterSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        
        private VibrationService _vibrationService;
        
        private EcsFilter<DeadRequest, ExplosionEnemyProvider> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var explosionEnemyProvider = ref entity.Get<ExplosionEnemyProvider>();

                Debug.Log($"ss {explosionEnemyProvider.ExplosionSourceMonoEntity.Entity}");
                EcsEntity expEntity = _world.NewEntity();
                explosionEnemyProvider.ExplosionSourceMonoEntity.Provide(ref expEntity);
                expEntity.Get<Stats>().Value = new StatValue();
                expEntity.Get<Stats>().Value[StatType.MiningDamage] = 11;
                expEntity.Get<Stats>().Value[StatType.Damage] = 5;
                expEntity.Get<Stats>().Value[StatType.PushForce] = 1;
                expEntity.Get<ExplosionRequest>().Radius = explosionEnemyProvider.Radius;

                _vibrationService.Vibrate(NiceHaptic.PresetType.MediumImpact);
            }
        }
    }
}