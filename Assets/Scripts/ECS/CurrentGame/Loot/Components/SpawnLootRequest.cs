using System.Collections.Generic;
using Client.Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public struct SpawnLootRequest
    {
        public Vector3 SpawnPosition;
        public List<Drop> Loot;
    }
}