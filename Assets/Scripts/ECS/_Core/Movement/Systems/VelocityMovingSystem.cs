using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class VelocityMovingSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter<VelocityMoving> _movingFilter;

        public void Run()
        {
            if (_movingFilter.IsEmpty())
                return;
            
            /*if (_data.RuntimeData.CurrentGameState != GameState.OnLevel)
                return;*/

            foreach (var movingObject in _movingFilter)
            {
                ref var movingEntity = ref _movingFilter.GetEntity(movingObject);
                ref GameObjectProvider movingEntityGo = ref movingEntity.Get<GameObjectProvider>();
                ref RigidbodyProvider movingEntityRb = ref movingEntity.Get<RigidbodyProvider>();

                ref var moving = ref movingEntity.Get<VelocityMoving>();

                moving.Speed = moving.Speed == 0 ? 2 : moving.Speed;
                moving.Accuracy = moving.Accuracy == 0 ? 0.1f : moving.Accuracy;

                movingEntityRb.Value.velocity =
                    (moving.Target.position + moving.Offset - movingEntityGo.Value.transform.position).normalized *
                    moving.Speed;

                if (Vector3.Distance(movingEntityGo.Value.transform.position, moving.Target.position + moving.Offset) <
                    moving.Accuracy)
                {
                    movingEntityRb.Value.velocity = Vector3.zero;
                    movingEntity.Del<VelocityMoving>();
                    movingEntity.Get<MovingCompleteEvent>();
                }
            }
        }
    }
}