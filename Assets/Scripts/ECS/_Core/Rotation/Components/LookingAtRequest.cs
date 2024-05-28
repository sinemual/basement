using UnityEngine;

namespace Client
{
    internal struct LookingAtRequest
    {
        public Vector3 Target;
        public float Accuracy;
        public float Speed;
    }
}