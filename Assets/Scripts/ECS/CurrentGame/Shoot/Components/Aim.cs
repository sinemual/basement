using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public struct Aim : IEcsSystem
    {
        public Vector2 Delta;
    }
}