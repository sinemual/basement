using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class ThrowTrajectorySystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<ThrowTrajectoryProvider, Aim> _filter;

        private Vector2 _mouseDelta;
        private Vector2 _mousePreviousPosition;
        private Vector2 _firstPosition;
        private Vector2 _secondPosition;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var lineRenderer = ref entity.Get<ThrowTrajectoryProvider>().LineRenderer;
                ref var targetObject = ref entity.Get<ThrowTrajectoryProvider>().TargetObject;
                ref var startPoint = ref entity.Get<ThrowTrajectoryProvider>().StartPoint;
                ref var delta = ref entity.Get<Aim>().Delta;
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;

                delta *= Time.deltaTime * _data.BalanceData.AimSpeed;
                startPoint.Rotate(Vector3.up, delta.x);
                //startPoint.Rotate(Vector3.right, -delta.y);
                
                /*var startVelocity = _data.BalanceData.ThrowStrength * startPoint.forward / _data.BalanceData.ThrowMass;

                int i = 0;
                for (float time = 0; time < _data.BalanceData.LinePoints; time += _data.BalanceData.TimeBetweenPoints)
                {
                    i++;
                    Vector3 point = startPoint.position + time * startVelocity;
                    point.y = startPoint.position.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                    lineRenderer.SetPosition(i, point);
                }*/
                
                lineRenderer.SetPosition(0, startPoint.position);

                //lastPointPosition += offset;
                //lineRenderer.SetPosition(lineRenderer.positionCount - 1, lastPointPosition);
                if (Physics.Raycast(startPoint.position, startPoint.forward, out var hit, 200.0f, _data.StaticData.BlocksMask))
                {
                    targetObject.transform.position = hit.point;
                    targetObject.transform.rotation = Quaternion.LookRotation(hit.normal);
                    lineRenderer.SetPosition(1, hit.point);
                }

                //Debug.DrawRay(perLastPointPosition,  direction,  Color.magenta);
            }
        }
    }
}