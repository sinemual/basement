using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public struct UiSpawnLootRequest
    {
        public Vector3 SpawnPosition;
        public List<Drop> Loot;
    }
}