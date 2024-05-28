using System.Linq;
using Client.Data.Core;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public class UiSpawnLootSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        private EcsFilter<UiSpawnLootRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var spawnLootRequest = ref entity.Get<UiSpawnLootRequest>();

                foreach (var dropItem in spawnLootRequest.Loot)
                {
                    for (int i = 0; i < dropItem.Amount; i++)
                    {
                        if (dropItem.Chance > Random.Range(0, 100))
                        {
                            var spawnGo = _prefabFactory.SpawnGo(dropItem.ItemData.View.UiDropItemPrefab,
                                spawnLootRequest.SpawnPosition, Quaternion.identity, _ui.gameObject.transform);

                            _world.NewEntity().Get<UiGoToPlayerRequest>() = new UiGoToPlayerRequest()
                            {
                                ItemData = dropItem.ItemData,
                                ItemGo = spawnGo
                            };
                        }
                    }
                }
                entity.Del<UiSpawnLootRequest>();
            }
        }
    }
}