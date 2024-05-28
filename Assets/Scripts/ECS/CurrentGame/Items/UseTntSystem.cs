using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class UseTntSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        
        private AudioService _audioService;
        
        private EcsFilter<UseHandItemRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var handItem = ref entity.Get<UseHandItemRequest>();

                if (handItem.Data is TntItemData tntItemData)
                {
                    handItem.ItemEntity.Get<ExplosionRequest>().Radius = tntItemData.ExplosionRadius;
                    _world.NewEntity().Get<ItemUsedEvent>().Value = handItem.Data;
                    entity.Del<UseHandItemRequest>();
                }
            }
        }
    }
}