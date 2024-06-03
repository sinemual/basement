using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Client.Infrastructure.Services;
using Data;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using CameraType = Client.Data.CameraType;
using Random = UnityEngine.Random;

namespace Client
{
    public class InitLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private CameraService _cameraService;
        private PrefabFactory _prefabFactory;
        private AnalyticService _analyticService;
        private AudioService _audioService;
        private UserInterfaceEventBus _uiEventBus;
        
        private EcsFilter<LevelProvider, CurrentLevelTag>.Exclude<InitedMarker> _levelFilter;
        
        private EcsFilter<SpawnPointDataProvider, ChestSpawnPointTagProvider> _chestsFilter;
        private EcsFilter<PlayerProvider, InitedMarker> _playerFilter;

        public void Run()
        {
            foreach (var currentLevel in _levelFilter)
            {
                ref var entity = ref _levelFilter.GetEntity(currentLevel);
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var level = ref entity.Get<LevelProvider>();

                entityGo.GetComponent<MonoEntitiesContainer>().ProvideMonoEntityChildren(_world);
                entity.Del<FromThisLevelTag>();

                _data.RuntimeData.CurrentGameState = GameState.OnLevel;

                _data.RuntimeData.ResetLevelData();
                CameraSetup(entityGo, level);
                PlayerSetup();
                InitChests();
                
                if (_data.RuntimeData.CurrentLocationType != LocationType.Tutorial && _data.PlayerData.IsRandomLevel)
                    level.CameraPointHandler.transform.rotation = Quaternion.Euler(0.0f, Utility.GetRandomTurnIslandAngle(), 0.0f);

                UiSetup();

                _analyticService.LogEvent("level_start");
                _audioService.StopAllAmbient();
                _audioService.Play(_data.AudioData.AmbientByType[_data.RuntimeData.CurrentLocationType]);

                entity.Get<InitedMarker>();
                _world.NewEntity().Get<LevelInitEvent>();
            }
        }

        private void PlayerSetup()
        {
            foreach (var idx in _playerFilter)
            {
                _playerFilter.GetEntity(idx).Get<Stats>().Value[StatType.Health] =
                    _playerFilter.GetEntity(idx).Get<Stats>().Value[StatType.FullHealth];

                _uiEventBus.PlayerDamage.OnInitPlayerHealthEvent(
                    _playerFilter.GetEntity(idx).Get<Stats>().Value[StatType.FullHealth],
                    _playerFilter.GetEntity(idx).Get<Stats>().Value[StatType.Health]);
            }
        }

        private void UiSetup()
        {
            //_ui.CameraControlScreen.SetShowState(true);
            _ui.OnLevelScreen.SetShowState(true);
            //_ui.PlayerStatusScreen.SetShowState(true);
            //_ui.OpenInventoryScreen.SetShowState(true);
            _ui.OpenCraftScreen.SetShowState(false);
            _ui.OpenTntPickaxeBoosterScreen.SetShowState(false);
            //_ui.CraftScreen.SetShowState(true);
            _ui.ChooseItemScreen.SetShowState(true);
        }

        private void CameraSetup(GameObject entityGo, LevelProvider level)
        {
            _cameraService.SetCamera(CameraType.LevelCamera, entityGo.transform, entityGo.transform);
            _cameraService.GetCurrentVC().transform.position = new Vector3(0.0f, 20.0f, -30.0f) + level.CameraOffset;
            _cameraService.GetCurrentVC().transform.SetParent(level.CameraPoint);
            level.CameraPoint.transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
            _cameraService.GetCurrentVC().transform.localRotation = Quaternion.Euler(new Vector3(30.0f, 0.0f, 0.0f));
            _cameraService.GetCurrentVC().m_Lens.OrthographicSize = level.CameraOrthoSize;
        }
        
        private void InitChests()
        {
            int randomChest = Random.Range(0, _chestsFilter.GetEntitiesCount());
            _chestsFilter.Get1(randomChest).Chance = 80;
        }
    }

    public struct LevelInitEvent
    {
    }

}