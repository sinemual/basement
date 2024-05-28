using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class MiningDestroySystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private PrefabFactory _prefabFactory;
        private VibrationService _vibrationService;
        private AudioService _audioService;

        private EcsFilter<BlockProvider, TimerDoneEvent<TimerToDisable>> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var blockType = ref entity.Get<BlockProvider>().Type;
                ref var blockLevel = ref entity.Get<BlockProvider>().Level;

                _world.NewEntity().Get<SpawnLootRequest>() = new SpawnLootRequest()
                {
                    SpawnPosition = entityGo.transform.position,
                    Loot = _data.StaticData.BlocksData[blockType].Levels[blockLevel].Loot
                };

                _world.NewEntity().Get<MinedEvent>().BlockType = blockType;
                if (entity.Has<TapProgressBar>())
                    _prefabFactory.Despawn(ref entity.Get<TapProgressBar>().Value);
                _prefabFactory.Despawn(ref entity);
            }
        }
    }
}