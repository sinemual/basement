using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class NavFollowMovementSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<NavMeshAgentProvider, NavFollowTarget, MovingState>.Exclude<MovingCompleteEvent> _checkFilter;
        private EcsFilter<NavMeshAgentProvider, NavFollowTarget, StartMovingRequest> _startRequestFilter;
        private EcsFilter<NavMeshAgentProvider, NavFollowTarget, StopMovingRequest> _stopRequestFilter;
        private EcsFilter<NavMeshAgentProvider, NavFollowTarget>.Exclude<MovingState> _updateFilter;


        public void Run()
        {
            foreach (var idx in _checkFilter)
            {
                ref var entity = ref _checkFilter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>();
                ref var navFollowTarget = ref entity.Get<NavFollowTarget>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                navMeshAgent.Value.SetDestination(navFollowTarget.Point.position);
                navMeshAgent.Value.speed = navFollowTarget.MoveSpeed;
                navMeshAgent.Value.angularSpeed = _data.BalanceData.PathMovementRotateSpeed;

                if (Vector3.Distance(navFollowTarget.Point.position, entityGo.Value.transform.position) < navFollowTarget.CompleteRadius)
                {
                    entity.Del<MovingState>();
                    entity.Get<MovingCompleteEvent>();
                }
            }

            foreach (var idx in _startRequestFilter)
            {
                ref var entity = ref _startRequestFilter.GetEntity(idx);
                ref var navFollowTarget = ref entity.Get<NavFollowTarget>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();

                if (!entityGo.Value.activeInHierarchy)
                    continue;

                navMeshAgent.Value.enabled = true;

                navMeshAgent.Value.SetDestination(navFollowTarget.Point.position);

                navMeshAgent.Value.speed = navFollowTarget.MoveSpeed;
                navMeshAgent.Value.angularSpeed = _data.BalanceData.PathMovementRotateSpeed;
                navMeshAgent.Value.acceleration = _data.BalanceData.PathMovementRotateSpeed;

                entity.Del<StartMovingRequest>();
                entity.Get<MovingState>();
            }

            foreach (var idx in _updateFilter)
            {
                ref var entity = ref _updateFilter.GetEntity(idx);
                ref var navFollowTarget = ref entity.Get<NavFollowTarget>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();

                if (!entityGo.Value.activeInHierarchy || !navMeshAgent.Value.enabled)
                    continue;

                navMeshAgent.Value.SetDestination(navFollowTarget.Point.position);

                if (Vector3.Distance(navFollowTarget.Point.position, entityGo.Value.transform.position) < navFollowTarget.CompleteRadius)
                    entity.Get<StartMovingRequest>();
            }

            foreach (var idx in _stopRequestFilter)
            {
                ref var entity = ref _stopRequestFilter.GetEntity(idx);
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                if (navMeshAgent.Value.hasPath)
                    navMeshAgent.Value.ResetPath();
                navMeshAgent.Value.enabled = false;

                entity.Del<MovingState>();
                entity.Del<StopMovingRequest>();
            }
        }
    }
}