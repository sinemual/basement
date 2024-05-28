using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InputJoystickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _gameData;
        private GameUI _gameUi;
        private Joystick _aimJoystick;
        private EcsWorld _world;

        private EcsFilter<JoystickInput, AimJoystickTag> _filter;

        public void Init()  
        {
            _aimJoystick = _gameUi.AimJoystick;
            EcsEntity snipeJoystickEntity = _world.NewEntity();
            snipeJoystickEntity.Get<JoystickInput>() = new JoystickInput
            {
                JoystickXPosition = 0.0f,
                JoystickYPosition = 0.0f,
                IsJoystickPointerDown = false
            };
            snipeJoystickEntity.Get<AimJoystickTag>();
        }

        public void Run()
        {
            foreach (var idx in _filter)
            {
                if (_filter.Get1(idx).IsJoystickPointerDown != _aimJoystick.IsPointerDown)
                {
                    if (_aimJoystick.IsPointerDown)
                        _filter.GetEntity(idx).Get<OnPointerDownEvent>();
                    else
                        _filter.GetEntity(idx).Get<OnPointerUpEvent>();
                }

                _filter.Get1(idx) = new JoystickInput
                {
                    JoystickXPosition = _aimJoystick.Horizontal,
                    JoystickYPosition = _aimJoystick.Vertical,
                    IsJoystickPointerDown = _aimJoystick.IsPointerDown
                };
            }

            //_aimJoystick.input = Vector2.zero;
        }
    }
}