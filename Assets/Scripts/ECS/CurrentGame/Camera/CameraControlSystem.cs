using Client.Data;
using Client.Data.Core;
using Client.DevTools.MyTools;
using Client.Infrastructure.Services;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Hit.Systems
{
    public class CameraControlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private CameraService _cameraService;
        private AudioService _audioService;
        private UserInterfaceEventBus _userInterfaceEventBus;
        private AnalyticService _analyticService;

        private EcsFilter<PlayerProvider>.Exclude<Timer<ReloadingTimer>> _playerFilter;
        private EcsFilter<CurrentLevelTag> _levelFilter;

        private Vector2 _mouseDelta;
        private Vector2 _mousePreviousPosition;
        private Vector2 _firstPosition;
        private Vector2 _secondPosition;
        private bool _isCameraBusy;

        public void Init()
        {
            _userInterfaceEventBus.CameraControlScreen.TurnRightButtonTap += () => SnapRotation(false);
            _userInterfaceEventBus.CameraControlScreen.TurnLeftButtonTap += () => SnapRotation(true);
        }

        public void Run()
        {
            if (_data.RuntimeData.CurrentGameState == GameState.GlobalMap)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _firstPosition = Input.mousePosition;
                    _mousePreviousPosition = Input.mousePosition;
                }

                _mouseDelta = Vector2.zero;
                if (Input.GetMouseButton(0))
                {
                    _mouseDelta = (Vector2)Input.mousePosition - _mousePreviousPosition;
                    _mousePreviousPosition = Input.mousePosition;
                    _mouseDelta.x /= Screen.width;
                    _mouseDelta.y /= Screen.height;
                    _mouseDelta *= 10000.0f;
                }
                
                if (Input.GetMouseButtonUp(0))
                {
                    _secondPosition = Input.mousePosition;
                    Vector2 mouseDelta = _secondPosition - _firstPosition;
                    //if (mouseDelta.x > 400)
                }
            }
            
            if (_data.RuntimeData.CurrentGameState != GameState.OnLevel)
                return;

            if (_isCameraBusy)
                return;

#if UNITY_EDITOR
            /*if (Input.GetKey(KeyCode.A))
                SnapRotation(true);
            if (Input.GetKey(KeyCode.D))
                SnapRotation(false);*/
#else
            /*if (Input.GetMouseButtonDown(0))
            {
                _firstPosition = Input.mousePosition;
                _mousePreviousPosition = Input.mousePosition;
            }

            _mouseDelta = (Vector2)Input.mousePosition - _mousePreviousPosition;
            _mousePreviousPosition = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                _secondPosition = Input.mousePosition;
                Vector2 mouseDelta = _secondPosition - _firstPosition;
                if (mouseDelta.x > 400)
                    SnapRotation(true);
                else if (mouseDelta.x < -400)
                    SnapRotation(false);
                return;
            }*/
#endif

            /*if (!Input.GetMouseButton(0))
                return;

            var position = Vector3.zero;
            /*var position = _cameraService.GetCurrentVC().transform.right *(_data.RuntimeData.MouseDelta.x * -_data.BalanceData.CameraMovementSpeed);#1#
            position += _cameraService.GetCurrentVC().transform.up * (_mouseDelta.y * -_data.BalanceData.CameraMovementSpeed);
            _cameraService.GetCurrentVC().transform.position += position * Time.deltaTime;*/
        }

        private void SnapRotation(bool isLeft)
        {
            _isCameraBusy = true;
            _audioService.Play(Sounds.CameraRotateSound);
            string eventDirection = isLeft ? "left" : "right";
            _analyticService.LogEventWithParameter("camera_rotate", eventDirection);

            var camPointT = _levelFilter.GetEntity(0).Get<LevelProvider>().CameraPoint.transform;

            _data.RuntimeData.CameraSide += isLeft ? 1 : -1;

            if (_data.RuntimeData.CameraSide > 3)
                _data.RuntimeData.CameraSide = 0;

            if (_data.RuntimeData.CameraSide < 0)
                _data.RuntimeData.CameraSide = 3;

            camPointT.DORotate(new Vector3(camPointT.rotation.x, Utility.IslandAngle[_data.RuntimeData.CameraSide], camPointT.rotation.z), 0.45f)
                .SetEase(Ease.InOutExpo)
                .OnComplete(() => { _isCameraBusy = false; });

            _world.NewEntity().Get<RotateCameraEvent>();
        }
    }
}