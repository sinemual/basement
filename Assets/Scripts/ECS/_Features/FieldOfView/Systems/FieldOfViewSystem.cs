using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class FieldOfViewSystem : IEcsRunSystem
    {
        private EcsFilter<FieldOfViewProvider>.Exclude<FieldOfViewInitedMarker> _initFilter;
        private EcsFilter<FieldOfViewProvider, TimerDoneEvent<TimerToUpdateFieldOfView>>.Exclude<DisableMarker> _updateFilter;

        public void Run()
        {
            foreach (var idx in _initFilter)
            {
                ref var entity = ref _initFilter.GetEntity(idx);
                ref var fieldOfViewProvider = ref entity.Get<FieldOfViewProvider>();
                fieldOfViewProvider.ViewMesh = new Mesh();
                fieldOfViewProvider.ViewMesh.name = "ViewMesh";
                fieldOfViewProvider.ViewMeshFilter.mesh = fieldOfViewProvider.ViewMesh;

                entity.Get<Timer<TimerToUpdateFieldOfView>>().Value = 0.2f;
                entity.Get<FieldOfViewInitedMarker>();
                entity.Get<DrawMaker>();
            }

            foreach (var idx in _updateFilter)
            {
                ref var entity = ref _updateFilter.GetEntity(idx);
                ref var fieldOfViewProvider = ref entity.Get<FieldOfViewProvider>();
                ref var fieldOfViewGo = ref entity.Get<GameObjectProvider>();

                fieldOfViewProvider.VisibleTargets.Clear();

                Collider[] targetsInViewRadius = Physics.OverlapSphere(fieldOfViewGo.Value.transform.position,
                    fieldOfViewProvider.ViewRadius, fieldOfViewProvider.TargetMask);
                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - fieldOfViewGo.Value.transform.position).normalized;
                    if (Vector3.Angle(fieldOfViewGo.Value.transform.forward, dirToTarget) <
                        fieldOfViewProvider.ViewAngle / 2)
                    {
                        float dstToTarget =
                            Vector3.Distance(fieldOfViewGo.Value.transform.position, target.position);
                        if (!Physics.Raycast(fieldOfViewGo.Value.transform.position, dirToTarget, dstToTarget,
                                fieldOfViewProvider.ObstacleMask))
                        {
                            fieldOfViewProvider.VisibleTargets.Add(target);
                        }
                    }
                }

                entity.Get<Timer<TimerToUpdateFieldOfView>>().Value = 0.2f;
            }
        }
    }
}