using Client.Data.Core;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;

namespace Client.ECS.CurrentGame.Mining
{
    public class UnderwaterKillSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private VibrationService _vibrationService;
        
        private EcsFilter<OnTriggerEnterEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var evnt = ref entity.Get<OnTriggerEnterEvent>();
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var characterType = ref entity.Get<CharacterProvider>().Type;
                
                
                if (evnt.Collider.transform.CompareTag(_data.StaticData.DespawnTag))
                {
                    entity.Get<DeadRequest>();
                }
            }
        }
    }
}