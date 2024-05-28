using Client.Data;
using Client.Data.Core;
using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class PlayerInputTapInteractSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private CameraService _cameraService;

        public void Run()
        {
            if (_data.RuntimeData.CurrentGameState != GameState.GlobalMap &&
                _data.RuntimeData.CurrentGameState != GameState.Village)
                return;
            
            if (Input.GetMouseButtonDown(0) && !Utility.IsPointerOverUIObject())
            {
                Ray ray = _cameraService.GetCamera().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                int mask = _data.RuntimeData.IsBlockingRaycastForTutorial ? _data.StaticData.TutorialMask : _data.StaticData.RaycastMask;
                if (Physics.Raycast(ray, out hit, 100, mask))
                    if (hit.transform.TryGetComponent(out MonoEntity hitEntity))
                        hitEntity.Entity.Get<InteractRequest>();
            }
        }
    }
}