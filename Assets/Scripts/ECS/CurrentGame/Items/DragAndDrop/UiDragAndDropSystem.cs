using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Loot.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class UiDragAndDropSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private CameraService _cameraService;
        private PrefabFactory _prefabFactory;

        private EcsFilter<PlayerProvider, HandItem, DragHandItemState> _playerFilter;

        public void Run()
        {
            foreach (var idx in _playerFilter)
            {
                ref var playerEntity = ref _playerFilter.GetEntity(idx);
                ref var handItem = ref playerEntity.Get<HandItem>();
                ref var handItemGo = ref handItem.Value.Get<GameObjectProvider>().Value;

                handItemGo.transform.position = _cameraService.GetCamera().ScreenToWorldPoint(Input.mousePosition);

                Ray dragRay = _cameraService.GetCamera().ScreenPointToRay(Input.mousePosition);
                RaycastHit dragHit;

                if (Physics.Raycast(dragRay, out dragHit, 100, _data.StaticData.RaycastMask))
                    handItemGo.transform.position = dragHit.transform.position + dragHit.normal;

                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = _cameraService.GetCamera().ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100, _data.StaticData.RaycastMask))
                    {
                        if (hit.transform.TryGetComponent(out MonoEntity hitEntity))
                        {
                            hitEntity.Entity.Get<UseHandItemRequest>() = new UseHandItemRequest()
                            {
                                ItemEntity = handItem.Value,
                                Data = handItem.Data
                            };
                            playerEntity.Del<HandItem>();
                        }
                    }
                    else
                    {
                        ItemData returnItem = handItem.Data;
                        playerEntity.Get<AddItemToInventoryRequest>().Value = returnItem;
                        
                        _prefabFactory.Despawn(ref handItem.Value);
                        playerEntity.Del<DragHandItemState>();
                    }
                }
            }
        }
    }
}