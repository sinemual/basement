using Client.Data;
using Client.Data.Core;
using Data;
using Leopotam.Ecs;

namespace Client
{
    public class AudioInitSystem : IEcsInitSystem
    {
        private SharedData _data;
        private AudioService _audioService;

        public void Init()
        {
            _audioService.ToggleAudio(_data.PlayerData.IsSoundOn);
            _audioService.Play(Sounds.FirstOstSound);
        }
    }
}