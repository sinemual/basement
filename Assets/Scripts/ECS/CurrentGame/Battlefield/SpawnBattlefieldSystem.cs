using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Mining;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class SpawnBattlefieldSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;
        private PrefabFactory _prefabFactory;

        private EcsFilter<BattlefieldProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var battlefield = ref entity.Get<BattlefieldProvider>();
                
                BattlefieldData battlefieldData = _data.StaticData.BattlefieldByLevelData[_data.PlayerData.CurrentWarStepIndex];

                int rightAmount = battlefieldData.SoldiersAmount[SoldierType.Warrior] + battlefieldData.SoldiersAmount[SoldierType.Archer];
                Vector3 rightSpawnPosition = battlefield.RightStartSpawnPoint.position;
                for (int i = 0; i < rightAmount; i++)
                {
                    EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.WolfPrefab, rightSpawnPosition + Vector3.forward * i,
                        Quaternion.identity);
                    spawnEntity.Get<SoldierTag>();
                    spawnEntity.Get<EnemyTag>();
                }
                
                int leftAmount = _data.PlayerData.SoldiersSaveData.Count;
                Vector3 leftSpawnPosition = battlefield.LeftStartSpawnPoint.position;
                for (int i = 0; i < leftAmount; i++)
                {
                    EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.PigPrefab, leftSpawnPosition + Vector3.forward * i,
                        Quaternion.identity);
                    spawnEntity.Get<SoldierTag>();
                }
                
                entity.Get<InitedMarker>();
            }
        }
    }
}