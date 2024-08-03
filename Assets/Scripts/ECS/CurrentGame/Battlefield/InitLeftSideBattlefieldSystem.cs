using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Mining;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitLeftSideBattlefieldSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;
        private PrefabFactory _prefabFactory;

        private EcsFilter<CharacterProvider, SoldierTag>.Exclude<InitedMarker, EnemyTag> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var battlefield = ref entity.Get<BattlefieldProvider>();

                foreach (var soldierData in _data.PlayerData.SoldiersSaveData)
                {
                    
                }
                
                entity.Get<InitedMarker>();
            }
        }
    }
}