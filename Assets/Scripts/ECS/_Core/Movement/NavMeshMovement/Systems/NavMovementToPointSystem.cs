using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class NavMovementToPointSystem : IEcsRunSystem
    {
        private SharedData _data;
        
        private EcsFilter<NavMeshAgentProvider, NavDestinationPoint, MovingState>.Exclude<MovingCompleteEvent> _checkFilter;
        private EcsFilter<NavMeshAgentProvider, NavDestinationPoint, MovingCompleteEvent> _completeFilter;
        private EcsFilter<NavMeshAgentProvider, NavDestinationPoint, StartMovingRequest> _startRequestFilter;
        private EcsFilter<NavMeshAgentProvider, NavDestinationPoint, StopMovingRequest> _stopRequestFilter;

        public void Run()
        {
            foreach (var idx in _startRequestFilter)
            {
                ref var entity = ref _startRequestFilter.GetEntity(idx);
                ref var point = ref entity.Get<NavDestinationPoint>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();

                if (!entityGo.Value.activeInHierarchy)
                    continue;

                navMeshAgent.Value.enabled = true;

                navMeshAgent.Value.SetDestination(point.Point.position);
                navMeshAgent.Value.speed = point.MoveSpeed;
                navMeshAgent.Value.angularSpeed = _data.BalanceData.PathMovementRotateSpeed;

                entity.Del<StartMovingRequest>();
                entity.Get<MovingState>();
            }

            foreach (var idx in _checkFilter)
            {
                ref var entity = ref _checkFilter.GetEntity(idx);
                ref var point = ref entity.Get<NavDestinationPoint>();
                ref GameObjectProvider entityGo = ref entity.Get<GameObjectProvider>();
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                navMeshAgent.Value.SetDestination(point.Point.position);
                navMeshAgent.Value.speed = point.MoveSpeed;
                navMeshAgent.Value.angularSpeed = _data.BalanceData.PathMovementRotateSpeed;

                if (Vector3.Distance(point.Point.position, entityGo.Value.transform.position) < point.CompleteRadius)
                    entity.Get<MovingCompleteEvent>();
            }

            foreach (var idx in _completeFilter)
            {
                ref var entity = ref _completeFilter.GetEntity(idx);
                ref var navMeshAgent = ref entity.Get<NavMeshAgentProvider>();

                if (navMeshAgent.Value.hasPath)
                    navMeshAgent.Value.ResetPath();
                navMeshAgent.Value.enabled = false;
                
                entity.Del<MovingState>();
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