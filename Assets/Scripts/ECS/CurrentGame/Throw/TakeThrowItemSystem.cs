using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TakeThrowItemSystem :IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;
        private UserInterfaceEventBus _uiEventBus;
        private PrefabFactory _prefabFactory;
        
        private EcsFilter<ThrowTrajectoryProvider> _trajectoryFilter;

        public void Init()
        {
            _uiEventBus.ChooseItemScreen.ChooseItemButtonTap += (itemData) =>
            {
                Transform startPoint = _trajectoryFilter.GetEntity(0).Get<ThrowTrajectoryProvider>().StartPoint;
                EcsEntity entity = _prefabFactory.Spawn(itemData.View.DropItemPrefab, startPoint.position, startPoint.rotation, startPoint);
                entity.Get<ThrowItem>().Value = itemData;
                entity.Get<ReadyMarker>();
            };
        }
    }
}