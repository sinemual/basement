using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.MonoBehaviour;
using Leopotam.Ecs;

namespace Client
{
    public class EndBurningVillageSceneSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        
        private CameraService _cameraService;
        private AudioService _audioService;
        
        private EcsFilter<EndBurningVillageSceneEvent> _filter;  

        public void Run()
        {
            foreach (var idx in _filter)
            {
                _ui.OpenSettingScreen.SetShowState(true);
                _data.SceneData.SceneBurningVillage.SetActive(false);
                _audioService.Play(Sounds.FirstOstSound);
                _data.RuntimeData.CurrentGameState = GameState.Village;
            }
        }
    }
}