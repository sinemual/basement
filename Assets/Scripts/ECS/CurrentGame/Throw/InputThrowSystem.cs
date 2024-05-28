using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InputThrowSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<JoystickInput, AimJoystickTag> _inputFilter;
        private EcsFilter<JoystickInput, AimJoystickTag, OnPointerUpEvent> _upFilter;
        private EcsFilter<ThrowTrajectoryProvider> _filter;

        public void Run()
        {
            if (_data.RuntimeData.CurrentGameState == GameState.OnLevel)
            {
                ref var joystickInput = ref _inputFilter.GetEntity(0).Get<JoystickInput>();
                
                if (joystickInput.IsJoystickPointerDown)
                {
                    _filter.GetEntity(0).Get<Aim>().Delta = new Vector2(joystickInput.JoystickXPosition, joystickInput.JoystickYPosition);
                }

                foreach (var idx in _upFilter)
                {
                        _filter.GetEntity(0).Get<ThrowRequest>();
                        _filter.GetEntity(0).Del<Aim>();
                }
            }
        }
    }
}