using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TouchInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter<InputTouchData, InputTouchLookData> _filter;

        public void Init()
        {
            EcsEntity inputTouchDataBus = _world.NewEntity();
            inputTouchDataBus.Get<InputTouchData>();
            inputTouchDataBus.Get<InputTouchLookData>();
        }

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var inputTouchData = ref _filter.GetEntity(idx).Get<InputTouchData>();
                ref var inputTouchLookData = ref _filter.GetEntity(idx).Get<InputTouchLookData>();

                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(0);
                    
                    inputTouchData.TouchPhase = touch.phase;
                    
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            inputTouchData.MoveTouchStartPosition = touch.position;
                            break;
                        case TouchPhase.Ended:
                            break;
                        case TouchPhase.Canceled:
                            break;
                        case TouchPhase.Moved:
                            inputTouchLookData.LookPosition = touch.deltaPosition * Time.deltaTime;
                            break;
                        case TouchPhase.Stationary:
                            inputTouchLookData.LookPosition = Vector2.zero;
                            break;
                    }
                }
            }
        }
    }
}