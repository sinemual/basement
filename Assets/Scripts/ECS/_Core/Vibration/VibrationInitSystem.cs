using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class VibrationInitSystem : IEcsInitSystem
    {
        private SharedData _data;

        public void Init() => Handheld.Vibrate();
    }
}