using Client.Data;
using Client.Data.Core;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class PathMovementSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<HasPath, StartMovingRequest> _startRequestFilter;
        private EcsFilter<HasPath, MovingCompleteEvent> _completeFilter;
        private EcsFilter<HasPath, StopMovingRequest> _stopRequestFilter;

        public void Run()
        {
            foreach (var idx in _startRequestFilter)
            {
                ref var entity = ref _startRequestFilter.GetEntity(idx);
                ref var hasPath = ref entity.Get<HasPath>();
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                if (entity.Has<AnimatorProvider>())
                    entity.Get<AnimatorProvider>().Value.SetBool(Animations.IsMoving, true);

                entity.Get<VelocityMoving>() = new VelocityMoving()
                {
                    Accuracy = hasPath.CompleteRadius,
                    Speed = hasPath.MovingSpeed,
                    Target = hasPath.Path.Value[hasPath.CurrentPathPointIndex]
                };

                if (!hasPath.IsNotFaceToDirection)
                    entityGo.transform.DOLookAt(hasPath.Path.Value[hasPath.CurrentPathPointIndex].position, 0.1f);

                entity.Del<StartMovingRequest>();
                entity.Get<MovingState>();
            }

            foreach (var idx in _stopRequestFilter)
            {
                ref var entity = ref _stopRequestFilter.GetEntity(idx);

                StopMoving(ref entity);
                entity.Del<StopMovingRequest>();
            }

            foreach (var idx in _completeFilter)
            {
                ref var entity = ref _completeFilter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>();
                ref var hasPath = ref entity.Get<HasPath>();

                StopMoving(ref entity);
                hasPath.CurrentPathPointIndex += entity.Has<GoToBackPathState>() ? -1 : 1;
                if (hasPath.CurrentPathPointIndex > hasPath.Path.Value.Count - 1)
                {
                    if (hasPath.Path.IsLoop)
                    {
                        if (hasPath.Path.IsGoToBack)
                        {
                            if (entity.Has<GoToBackPathState>())
                            {
                                hasPath.CurrentPathPointIndex = 0;
                                entity.Del<GoToBackPathState>();
                            }
                            else
                                entity.Get<GoToBackPathState>();
                        }
                        else
                            hasPath.CurrentPathPointIndex = 0;

                        if (hasPath.Path.IsTeleportToBeginPath)
                            entityGo.Value.transform.position = hasPath.Path.Value[hasPath.CurrentPathPointIndex].position;

                        entity.Get<StartMovingRequest>();
                    }
                    else
                    {
                        continue;
                    }
                }

                entity.Get<StartMovingRequest>();
            }
        }

        private void StopMoving(ref EcsEntity entity)
        {
            entity.Del<VelocityMoving>();
            entity.Del<MovingState>();

            if (entity.Has<AnimatorProvider>())
                entity.Get<AnimatorProvider>().Value.SetBool(Animations.IsMoving, false);
        }
    }

    public struct GoToBackPathState : IEcsIgnoreInFilter
    {
    }
}