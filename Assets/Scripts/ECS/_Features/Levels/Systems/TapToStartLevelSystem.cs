using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TapToStartLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        public void Run()
        {
            /*if (_gameData.RuntimeData.CurrentGameState == StaticData.Enums.GameState.LevelStart)
                if (Input.GetMouseButton(0))*/
        }
    }
}