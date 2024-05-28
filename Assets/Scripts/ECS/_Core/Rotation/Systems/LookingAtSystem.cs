using Client.DevTools.MyTools;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class LookingAtSystem : IEcsRunSystem
    {
        private EcsFilter<LookingAtRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();

                ref var lookingAt = ref entity.Get<LookingAtRequest>();

                lookingAt.Speed = lookingAt.Speed == 0 ? 1 : lookingAt.Speed;
                lookingAt.Accuracy = lookingAt.Accuracy == 0 ? 0.05f : lookingAt.Accuracy;

                var targetRotation = Quaternion.LookRotation(lookingAt.Target - entityGo.Value.transform.position)
                    .normalized;

                entityGo.Value.transform.rotation =
                    Quaternion.Slerp(entityGo.Value.transform.rotation, targetRotation, lookingAt.Speed);

                if (Utility.ApproximatelyQuaternions(entityGo.Value.transform.rotation, targetRotation, lookingAt.Accuracy))
                    entity.Del<LookingAtRequest>();
            }
        }
    }
}