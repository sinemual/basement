using UnityEngine;

namespace Client
{
    internal struct LaunchRequest
    {
        public Transform LaunchPoint;
        public Vector2 Force;
        public bool IsRandomDirection;
    }
}