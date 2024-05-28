using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    internal struct SpawnData
    {
        public int Id;
        public EcsEntity SpawnEntity;
        public Transform Point;
        public SpawnPointProvider SpawnPointProvider;
    }
}