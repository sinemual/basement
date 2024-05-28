using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class NavPathMovementSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<NavMeshAgentProvider, HasPath, StartMovingRequest> _startRequestFilter;
        private EcsFilter<NavMeshAgentProvider, HasPath, MovingState>.Exclude<MovingCompleteEvent> _checkFilter;
        private EcsFilter<NavMeshAgentProvider, HasPath, MovingCompleteEvent> _completeFilter;
        private EcsFilter<NavMeshAgentProvider, HasPath, StopMovingRequest> _stopRequestFilter;

        public void Run()
        {
            foreach (var idx in _checkFilter)
            {
                ref var entity = ref _checkFilter.GetEntity(idx);
                ref HasPath hasPath = ref entity.Get<HasPath>();
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                if (navMeshAgent.Value.enabled)
                    if (Vector3.Distance(hasPath.Path.Value[hasPath.CurrentPathPointIndex].position, entityGo.Value.transform.position) <
                        hasPath.CompleteRadius)
                        entity.Get<MovingCompleteEvent>();
            }

            foreach (var idx in _startRequestFilter)
            {
                ref var entity = ref _startRequestFilter.GetEntity(idx);
                ref HasPath hasPath = ref entity.Get<HasPath>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();

                if (!entityGo.Value.activeInHierarchy)
                    continue;

                navMeshAgent.Value.enabled = true;

                navMeshAgent.Value.SetDestination(hasPath.Path.Value[hasPath.CurrentPathPointIndex].position);
                navMeshAgent.Value.speed = hasPath.MovingSpeed;
                navMeshAgent.Value.angularSpeed = _data.BalanceData.PathMovementRotateSpeed;
                navMeshAgent.Value.acceleration = _data.BalanceData.PathMovementRotateSpeed;

                entity.Del<StartMovingRequest>();
                entity.Get<MovingState>();
            }

            foreach (var idx in _stopRequestFilter)
            {
                ref var entity = ref _stopRequestFilter.GetEntity(idx);
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                if (navMeshAgent.Value.hasPath)
                    navMeshAgent.Value.ResetPath();

                navMeshAgent.Value.enabled = false;

                entity.Del<StopMovingRequest>();
                entity.Del<MovingState>();
            }

            foreach (var idx in _completeFilter)
            {
                ref var entity = ref _completeFilter.GetEntity(idx);
                ref HasPath hasPath = ref entity.Get<HasPath>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                hasPath.CurrentPathPointIndex++;
                if (hasPath.CurrentPathPointIndex > hasPath.Path.Value.Count - 1)
                {
                    if (hasPath.Path.IsLoop)
                    {
                        hasPath.CurrentPathPointIndex = 0;
                    }
                    else
                    {
                        if (navMeshAgent.Value.hasPath)
                        {
                            navMeshAgent.Value.ResetPath();
                            navMeshAgent.Value.SetDestination(hasPath.Path.Value[hasPath.CurrentPathPointIndex].position);
                        }

                        navMeshAgent.Value.enabled = false;
                        entity.Del<MovingState>();
                        continue;
                    }
                }

                if (navMeshAgent.Value.enabled)
                    navMeshAgent.Value.SetDestination(hasPath.Path.Value[hasPath.CurrentPathPointIndex].position);
            }
        }
    }
}