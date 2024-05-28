using Client.Data;
using Client.Data.Core;
using Client.DevTools.MyTools;
using Client.ECS.CurrentGame.Hit.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class PlayerInputTapHitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private CameraService _cameraService;

        private EcsFilter<PlayerProvider>.Exclude<DragHandItemState> _playerFilter;
        private EcsFilter<TouchInput>.Exclude<Timer<ReloadingTimer>> _touchesFilter;

        public void Init()
        {
            EcsEntity firstTouch = _world.NewEntity();
            firstTouch.Get<TouchInput>().Index = 0;
            firstTouch.Get<Timer<ReloadingTimer>>().Value = 0;

            /*EcsEntity secondTouch = _world.NewEntity();
            secondTouch.Get<TouchInput>().Index = 1;
            secondTouch.Get<Timer<ReloadingTimer>>().Value = 0;
            
            EcsEntity thirdTouch = _world.NewEntity();
            thirdTouch.Get<TouchInput>().Index = 2;
            thirdTouch.Get<Timer<ReloadingTimer>>().Value = 0;*/
        }

        public void Run()
        {
            if (_data.RuntimeData.CurrentGameState != GameState.OnLevel)
                return;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !Utility.IsPointerOverUIObject())
            {
                Ray ray = _cameraService.GetCamera().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                int mask = _data.RuntimeData.IsBlockingRaycastForTutorial ? _data.StaticData.TutorialMask : _data.StaticData.RaycastMask;
                if (Physics.Raycast(ray, out hit, 100, mask))
                    if (hit.transform.TryGetComponent(out MonoEntity hitEntity))
                        hitEntity.Entity.Get<HitRequest>().HitterEntity = _playerFilter.GetEntity(0);
            }

#else
            foreach (var touch in Input.touches)
            {
                if (!_playerFilter.IsEmpty())
                    if (touch.fingerId == _touchesFilter.Get1(0).Index && !_touchesFilter.GetEntity(0).Has<Timer<ReloadingTimer>>() && !Utility.IsPointerOverUIObject())
                    {
                        Ray ray = _cameraService.GetCamera().ScreenPointToRay(touch.position);
                        RaycastHit hit;

                        int mask = _data.RuntimeData.IsBlockingRaycastForTutorial ? _data.StaticData.TutorialMask : _data.StaticData.RaycastMask;
                        if (Physics.Raycast(ray, out hit, 100, mask))
                        {
                            if (hit.transform.TryGetComponent(out MonoEntity hitEntity))
                            {
                                _touchesFilter.GetEntity(0).Get<Timer<ReloadingTimer>>().Value = _data.BalanceData.TapReloadTime;
                                hitEntity.Entity.Get<HitRequest>().HitterEntity = _playerFilter.GetEntity(0);
                            }
                        }
                    }
            }
#endif
        }

        private void DebugDrawRay()
        {
            ref var playerGo = ref _playerFilter.GetEntity(0).Get<GameObjectProvider>().Value;
            var playerT = playerGo.transform;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 100f;
            mousePos = _cameraService.GetCamera().ScreenToWorldPoint(mousePos);
            Debug.DrawRay(playerT.position, mousePos - playerT.position, Color.blue);
        }
    }

    public struct TouchInput
    {
        public int Index;
    }
}