using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Client;
using Client.Data.Core;
using Client.ECS.CurrentGame.Experience;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.ECS.CurrentGame.Mining;
using Client.ECS.CurrentGame.PlayerEquipment;
using Client.Infrastructure.MonoBehaviour;
using Client.Infrastructure.Services;
using DG.Tweening;
using Leopotam.Ecs;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
#endif
using UnityEngine;

namespace Client
{
    internal sealed class Game : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private SharedData _data;
        [SerializeField] private GameUI _ui;

        private TimeManagerService _timeManagerService;
        private CameraService _cameraService;
        private AudioService _audioService;
        private AnalyticService _analyticService;
        private FirebaseRemoteConfigService _firebaseRemoteConfigService;
        private InternetAccessStateService _internetAccessStateService;
        private AdsService _adsService;
        private VibrationService _vibrationService;
        private PrefabFactory _prefabFactory;
        private PoolService _poolService;
        private CleanService _cleanService;
        private LoadGameService _loadGameService;
        private SlowMotionService _slowMotionService;
        private UserInterfaceEventBus _uiEventBus;

        private EcsWorld _ecsWorld;

        private EcsSystems _updateSystems;
        private EcsSystems _lateUpdateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Awake()
        {
            _data.SceneData.LoadingUi.SetActive(true);
#if !UNITY_EDITOR
            StartCoroutine(LoadSDK());
#else
            LoadGame();
#endif
        }

        private IEnumerator LoadSDK()
        {
            yield return WaitSDK();
            LoadGame();
        }

        private IEnumerator WaitSDK()
        {
            var t = 0.0f;
            while (t < 5.0f /*&& !SDK.Initialized*/)
            {
                t += Time.deltaTime;
                yield return null;
            }
        }

        private async void LoadGame()
        {
            Debug.Log($"LoadGame");
            _data.ManualStart();
            _timeManagerService = new TimeManagerService(_data, this);
            _internetAccessStateService = new InternetAccessStateService();
            _firebaseRemoteConfigService = new FirebaseRemoteConfigService();
            _loadGameService = new LoadGameService(_data, _ui);
            await _loadGameService.StartGameLoad();
            StartCoroutine(ManualStart());
        }

        private IEnumerator ManualStart()
        {
            Debug.Log($"StartGame");
            _data.SceneData.LoadingUi.SetActive(false);

            _ecsWorld = new EcsWorld();
            _updateSystems = new EcsSystems(_ecsWorld, " - UPDATE");
            _lateUpdateSystems = new EcsSystems(_ecsWorld, " - LATE UPDATE");
            _fixedUpdateSystems = new EcsSystems(_ecsWorld, " - FIXED UPDATE");

#if UNITY_EDITOR
            EcsWorldObserver.Create(_ecsWorld);
            EcsSystemsObserver.Create(_updateSystems);
            EcsSystemsObserver.Create(_lateUpdateSystems);
            EcsSystemsObserver.Create(_fixedUpdateSystems);
#endif
            //---Services---
            _audioService = new AudioService(_data);
            _analyticService = new AnalyticService(_data);
            _adsService = new AdsService(_ecsWorld, _data, _analyticService, _timeManagerService);
            _poolService = new PoolService(_data);
            _slowMotionService = new SlowMotionService(this);
            _cleanService = new CleanService(_poolService);
            _cameraService = new CameraService(_data.SceneData.CameraSceneData, this);
            _prefabFactory = new PrefabFactory(_ecsWorld, null, _poolService, _cleanService);
            _uiEventBus = new UserInterfaceEventBus();
            _vibrationService = new VibrationService(_data);

            //---Injects---
            _ui.Inject(_data, _uiEventBus, _timeManagerService, _audioService, _analyticService, _adsService);

            _analyticService.LogEvent("start_game");
            SetTargetFrameRate();
            ProvideMonoEntitiesFromScene();
            //DOTween.SetTweensCapacity(500, 125);

            //---SystemGroups---
            //var inputSystems = InputSystems();
            var spawnSystems = SpawnSystems();
            var movementSystems = MovementSystems();
            var physicMovementSystems = PhysicMovementSystems();
            var timerSystems = TimerSystems();
            var userInterfaceSystems = UserInterfaceSystems();

            _updateSystems
                //---General---
                .Add(userInterfaceSystems)
                .Add(timerSystems)
                //.Add(new FirebaseConfigSystem())
                .Add(new ShortcutCheatSystem())
                .Add(new RestartGameSystem())
                .Add(new InitGameSystem())
                .Add(new DisposeLevelSystem())
                .Add(new GameStateSystem())
                .Add(new GameOverSystem())
                .Add(new VibrationInitSystem())
                .Add(new AudioInitSystem())
                .Add(new CameraControlSystem())
                .Add(new RagdollSystem())
                //---Throw---
                //.Add(new TakeThrowItemSystem())
                .Add(new InputJoystickSystem())
                .Add(new InputThrowSystem())
                .Add(new ThrowTrajectorySystem())
                .Add(new ThrowSystem())
                .Add(spawnSystems)
                .Add(movementSystems)
                //------GLOBAL MAP------
                .Add(new GlobalMapSystem())
                .Add(new PlayerInputTapInteractSystem())
                //------VILLAGE------
                .Add(new EndBurningVillageSceneSystem())
                .Add(new BuildSystem())
                .Add(new BuildingProduceItemSystem())
                .Add(new BuildingTakeProductItemSystem())
                .Add(new VillageUpdateSystem())
                .Add(new OpenRecipesSystem())
                .Add(new RecipesGoToPlayerSystem())
                .Add(new InteractVillagerSystem())
                //------LEVELS------
                .Add(new InitLevelSystem())
                //.Add(new CalculateExperienceLevelSystem())
                //---Init---
                .Add(new InitBlockSystem())
                .Add(new CraftSystem())
                .Add(new InitStatsSystem())
                //---Boosters---
                .Add(new ShowTntPickaxeBoosterOfferSystem())
                //---Player---
                .Add(new InitPlayerOnLevelSystem())
                .Add(new PlayerInputTapHitSystem())
                .Add(new EquipCraftedItemSystem())
                .Add(new AddToInventoryCraftedItemSystem())
                .Add(new InventorySystem())
                .Add(new EquipItemSystem())
                .Add(new EquipHandItemSystem())
                .Add(new RecalculateStatsSystem())
                //---Items---
                .Add(new CreateDragItemSystem())
                .Add(new UiDragAndDropSystem())
                .Add(new UseTntSystem())
                .Add(new ExplosionSystem())
                .Add(new HitChestSystem())
                .Add(new ChestFlySystem())
                .Add(new UsedItemSystem())
                //---Mining---
                .Add(new MiningSystem())
                .Add(new MiningFeelingSystem())
                .Add(new MiningDestroySystem())
                .Add(new ExplosionMiningDestroySystem())
                //---Fighting---
                .Add(new InitEnemySystem())
                .Add(new EnemyTakeDamageSystem())
                .Add(new EnemyCheckPlayerSystem())
                .Add(new EnemyStartShootPlayerSystem())
                .Add(new EnemyShootPlayerSystem())
                //.Add(new DamageDropPlayerResourcesSystem())
                .Add(new ExplosionEnemyTakeDamageSystem())
                .Add(new ForceImpactSystem())
                .Add(new TntPickaxeBoosterSystem())
                //---Loot---
                .Add(new SpawnLootSystem())
                .Add(new ResourceGoToPlayerSystem())
                .Add(new ExperienceGoToPlayerSystem())
                .Add(new UsableItemGoToPlayerSystem())
                //---Loot_UI---
                .Add(new UiSpawnLootSystem())
                .Add(new UiResourceGoToPlayerSystem())
                .Add(new UiUsableItemGoToPlayerSystem())
                .Add(new MineTapProgressBarSystem())
                .Add(new HitTapProgressBarSystem())
                //---Health---
                .Add(new UnderwaterKillSystem())
                .Add(new ExplosionEnemySystem())
                .Add(new DeathSystem())
                //---Calculates---
                .Add(new ExperienceSystem())
                .Add(new CalculateResourceSystem())
                .Add(new CheckProgressGoalCompleteSystem())
                //---Tutorial---
                //.Add(new CheckProgressTutorialSystem())
                //.Add(new TutorialSystem())
                //.Add(new StartTutorialSystem())
                //---GlobalMap2---
                .Add(new OpenLocationGlobalMapSystem())
                //.Add(new ShowRateUsPopupSystem())
                //---Events---
                .Add(new CatchEventSystem())
                //---OneFrames---
                .OneFrame<MovingCompleteEvent>()
                .OneFrame<DeadEvent>()
                .OneFrame<MineEvent>()
                .OneFrame<HitEvent>()
                .OneFrame<MinedEvent>()
                .OneFrame<BuildEvent>()
                .OneFrame<OnPointerUpEvent>()
                .OneFrame<ChestFoundEvent>()
                .OneFrame<PickBuildEvent>()
                .OneFrame<LevelInitEvent>()
                .OneFrame<PlayerDeathEvent>()
                .OneFrame<ItemUsedEvent>()
                .OneFrame<CheckGoalStateEvent>()
                .OneFrame<EarnCurrencyEvent>()
                .OneFrame<LevelCompleteEvent>()
                .OneFrame<RotateCameraEvent>()
                .OneFrame<TutorialCompleteEvent>()
                .OneFrame<GameStateChangedEvent>()
                .OneFrame<LevelGoalCompleteEvent>()
                .OneFrame<EndBurningVillageSceneEvent>()
                .OneFrame<TakeProgressRewardEvent>()
                .OneFrame<FirebaseRemoteConfigLastFetchSuccessEvent>()
                //---PhysicOneFrames---
                .OneFrame<OnCollisionEnterEvent>()
                .OneFrame<OnTriggerEnterEvent>()
                .OneFrame<OnTriggerExitEvent>()
                .OneFrame<RaycastEvent>()
                .OneFrame<ItemCraftedEvent>()
                //---Injects---
                .Inject(this) // for coroutine runner
                .Inject(_uiEventBus)
                .Inject(_data)
                .Inject(_ui)
                .Inject(_slowMotionService)
                .Inject(_analyticService)
                .Inject(_internetAccessStateService)
                .Inject(_firebaseRemoteConfigService)
                .Inject(_adsService)
                .Inject(_cameraService)
                .Inject(_audioService)
                .Inject(_prefabFactory)
                .Inject(_timeManagerService)
                .Inject(_vibrationService)
                .Init();

            _lateUpdateSystems
                .Inject(_cameraService)
                .Inject(_data)
                .Init();

            _fixedUpdateSystems
                .Add(physicMovementSystems)
                //---Injects---
                .Inject(_ui)
                .Inject(_data)
                .Inject(_cameraService)
                .Inject(_analyticService)
                .Init();

            yield return null;
        }

        private void Update() => _updateSystems?.Run();

        private void LateUpdate() => _lateUpdateSystems?.Run();

        private void FixedUpdate() => _fixedUpdateSystems?.Run();

        private void OnDestroy()
        {
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;

                _lateUpdateSystems.Destroy();
                _lateUpdateSystems = null;

                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;

                _ecsWorld.Destroy();
                _ecsWorld = null;
            }
        }

        //------------------SYSTEM GROUPS---------------

        private EcsSystems SpawnSystems()
        {
            return new EcsSystems(_ecsWorld, "SpawnSystems")
                .Add(new DespawnAtTimerSystem())
                .Add(new TryDespawnBusyWorldUiAtTimerSystem())
                //spawn
                .Add(new SpawnLevelSystem())
                .Add(new SpawnPointSystem());
        }

        private EcsSystems MovementSystems()
        {
            return new EcsSystems(_ecsWorld, "MovementSystems")
                .Add(new InitPathMovementSystem())
                .Add(new PathMovementSystem())
                .Add(new TransformMovingSystem())
                .Add(new LookingAtSystem());
        }

        private EcsSystems PhysicMovementSystems()
        {
            return new EcsSystems(_ecsWorld, "PhysicMovementSystems")
                .Add(new VelocityMovingSystem())
                .Add(new PhysicForceAddSystem())
                .Add(new PhysicForceAddToPointSystem())
                .Add(new PhysicAddExplosionForceSystem())
                .Add(new LaunchSystem())
                .Add(new LandingSystem());
        }

        private EcsSystems TimerSystems()
        {
            return new EcsSystems(_ecsWorld, "TimerSystems")
                .Add(new TimerSystem<TimerToEnable>())
                .Add(new TimerSystem<TimerToDisable>())
                .Add(new TimerSystem<TimerToDeadDespawn>())
                .Add(new TimerSystem<DespawnTimer>())
                .Add(new TimerSystem<TimerToHideLootScreen>())
                .Add(new TimerSystem<TimeToProduce>())
                .Add(new TimerSystem<TimerToFly>())
                .Add(new TimerSystem<InterReloadTimer>())
                .Add(new TimerSystem<ReloadingTimer>());
        }

        private EcsSystems UserInterfaceSystems()
        {
            return new EcsSystems(_ecsWorld, "UserInterfaceSystems")
                .Add(new LevelCompleteScreenInputSystem())
                .Add(new TntPickaxeScreenInputSystem())
                .Add(new NoAdsIapScreenInputSystem())
                .Add(new GlobalMapScreenInputSystem())
                .Add(new ExitFromLevelScreenInputSystem())
                .Add(new OpenChestScreenInputSystem())
                .Add(new GoalScreenInputSystem())
                .Add(new HandItemScreenInputSystem())
                .Add(new VillageScreenInputSystem())
                .Add(new LevelFailedScreenInputSystem())
                .Add(new InventoryScreenInputSystem())
                .Add(new SettingScreenInputSystem());
        }

        private static void SetTargetFrameRate() => Application.targetFrameRate = 60;

        private void ProvideMonoEntitiesFromScene()
        {
            List<MonoEntity> monoEntities = new List<MonoEntity>();
            monoEntities.AddRange(FindObjectsOfType<MonoEntity>(true));
            foreach (var monoEntity in monoEntities)
            {
                var ecsEntity = _ecsWorld.NewEntity();
                monoEntity.Provide(ref ecsEntity);
            }
        }

        private void OnApplicationQuit()
        {
            _analyticService.LogEvent("quit_game");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                _analyticService.LogEvent("quit_game");
            else if (_analyticService != null)
                _analyticService.LogEvent("resume_game");
        }
    }
}