using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class CreateDragItemSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        
        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private AudioService _audioService;
        
        private EcsFilter<CreateDragItemRequest> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref ItemData itemData = ref entity.Get<CreateDragItemRequest>().Value;
                
                _ui.HandItemScreen.TakeItem(itemData);
                
                EcsEntity createdEntity = _prefabFactory.Spawn(itemData.View.ItemPrefab,
                    _cameraService.GetCamera().ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

                _playerFilter.GetEntity(0).Get<HandItem>() = new HandItem()
                {
                    Value = createdEntity,
                    Data = itemData
                };

                if (itemData is TntItemData tnt)
                {
                    _audioService.Play(Sounds.FuseSound);
                    createdEntity.Get<Stats>().Value = new StatValue();
                    foreach (var stat in tnt.Stats)
                        createdEntity.Get<Stats>().Value[stat.Key] = stat.Value;
                }
                
                entity.Del<CreateDragItemRequest>();
            }
        }
    }
}