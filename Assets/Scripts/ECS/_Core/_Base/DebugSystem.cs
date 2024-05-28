using System.Collections;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class DebugSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;
        private ICoroutineRunner _runner;

        public void Init()
        {
            _runner.StartCoroutine(b());
            //Debug.Log($"_gameData {_gameData}");
        }

        public void Run()
        {
            //Debug.Log($"_gameData {_gameData}");
        }

        IEnumerator b()
        {
            yield return new WaitForSeconds(3.11f);
            Debug.Log($"ooorr {_data}");
        }
    }
}