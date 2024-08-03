using System;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class LoadArrowSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;
        private UserInterfaceEventBus _uiEventBus;
        private PrefabFactory _prefabFactory;
        
        private EcsFilter<ThrowTrajectoryProvider, OnPointerDownEvent> _inputFilter;

        public void Run()
        {
            foreach (var idx in _inputFilter)
            {
                Transform startPoint = _inputFilter.GetEntity(0).Get<ThrowTrajectoryProvider>().StartPoint;
                EcsEntity entity = _prefabFactory.Spawn(_data.StaticData.PrefabData.ArrowPrefab, startPoint.position, startPoint.rotation, startPoint);
                entity.Get<ReadyMarker>();
            }
        }
    }
}