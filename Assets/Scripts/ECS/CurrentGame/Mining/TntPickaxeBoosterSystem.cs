using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Hit.Components;
using Client.Infrastructure.Services;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class TntPickaxeBoosterSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private VibrationService _vibrationService;
        private PrefabFactory _prefabFactory;

        private EcsFilter<BlockProvider, MineEvent> _mineFilter;
        private EcsFilter<EnemyProvider, HitEvent>.Exclude<DeadState> _enemyFilter;

        public void Run()
        {
            if (_data.RuntimeData.IsTntPickaxeBoosterWork)
            {
                foreach (var idx in _mineFilter)
                {
                    ref var entity = ref _mineFilter.GetEntity(idx);
                    ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                    CreateExplosion(entityGo.transform.position);
                }
                
                foreach (var idx in _enemyFilter)
                {
                    ref var entity = ref _enemyFilter.GetEntity(idx);
                    ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                    CreateExplosion(entityGo.transform.position);
                }
            }
        }

        private void CreateExplosion(Vector3 position)
        {
            EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.TntPickaxeExplosionSourcePrefab,
                position, Quaternion.identity);

            spawnEntity.Get<Stats>().Value = new StatValue();
            spawnEntity.Get<Stats>().Value.Add(StatType.MiningDamage, 5f);
            spawnEntity.Get<Stats>().Value.Add(StatType.Damage, 2.5f);
            spawnEntity.Get<Stats>().Value.Add(StatType.PushForce, 2.5f);
                    
            spawnEntity.Get<ExplosionRequest>().Radius = 1f;
                    
            spawnEntity.Get<DespawnAtTimerRequest>();
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.DespawnMiningViewTime;
        }
    }
}