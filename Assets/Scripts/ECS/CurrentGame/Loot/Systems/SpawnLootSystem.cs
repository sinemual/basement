using Client.Data;
using Client.Data.Core;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public class SpawnLootSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        
        private PrefabFactory _prefabFactory;
        
        private EcsFilter<SpawnLootRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var spawnLootRequest = ref entity.Get<SpawnLootRequest>();

                foreach (var dropItem in spawnLootRequest.Loot)
                {
                    for (int i = 0; i < dropItem.Amount; i++)
                    {
                        if (dropItem.Chance > Random.Range(0, 100))
                        {
                            var spawnGo = _prefabFactory.SpawnGo(dropItem.ItemData.View.DropItemPrefab,
                                spawnLootRequest.SpawnPosition, Quaternion.identity);

                            _world.NewEntity().Get<GoToPlayerRequest>() = new GoToPlayerRequest()
                            {
                                ItemData = dropItem.ItemData,
                                ItemGo = spawnGo
                            };
                        }
                    }
                }
                entity.Del<SpawnLootRequest>();
            }
        }
    }
}