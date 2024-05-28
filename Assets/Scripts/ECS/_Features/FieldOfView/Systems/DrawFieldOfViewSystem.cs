using System.Collections.Generic;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class DrawFieldOfViewSystem : IEcsRunSystem
    {
        private EcsFilter<FieldOfViewProvider, FieldOfViewInitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var fieldOfViewProvider = ref entity.Get<FieldOfViewProvider>();
                ref var fieldOfViewGo = ref entity.Get<GameObjectProvider>();

                DrawFieldOfView(ref fieldOfViewProvider, fieldOfViewGo.Value.transform);
            }
        }
        
        private void DrawFieldOfView(ref FieldOfViewProvider fieldOfViewProvider, Transform transform)
        {
            int stepCount = Mathf.RoundToInt(fieldOfViewProvider.ViewAngle * fieldOfViewProvider.MeshResolution);
            float stepAngleSize = fieldOfViewProvider.ViewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();
            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - fieldOfViewProvider.ViewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle, transform, fieldOfViewProvider.ObstacleMask,
                    fieldOfViewProvider.ViewRadius);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) >
                                                    fieldOfViewProvider.EdgeDstThreshold;

                    if (oldViewCast.hit != newViewCast.hit ||
                        (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast,
                            fieldOfViewProvider.EdgeResolveIterations,
                            fieldOfViewProvider.EdgeDstThreshold, transform, fieldOfViewProvider.ObstacleMask,
                            fieldOfViewProvider.ViewRadius);
                        if (edge.pointA != Vector3.zero)
                            viewPoints.Add(edge.pointA);

                        if (edge.pointB != Vector3.zero)
                            viewPoints.Add(edge.pointB);
                    }
                }


                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) +
                                  Vector3.forward * fieldOfViewProvider.MaskCutawayDst;

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            fieldOfViewProvider.ViewMesh.Clear();

            fieldOfViewProvider.ViewMesh.vertices = vertices;
            fieldOfViewProvider.ViewMesh.triangles = triangles;
            fieldOfViewProvider.ViewMesh.RecalculateNormals();
        }
        
        private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, int edgeResolveIterations,
            float edgeDstThreshold, Transform transform, LayerMask obstacleMask, float viewRadius)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle, transform, obstacleMask, viewRadius);

                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }


        private ViewCastInfo ViewCast(float globalAngle, Transform transform, LayerMask obstacleMask,
            float viewRadius)
        {
            Vector3 dir = DirFromAngle(globalAngle, true, transform);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
            }
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal, Transform transform)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }

        public struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }
    }
}