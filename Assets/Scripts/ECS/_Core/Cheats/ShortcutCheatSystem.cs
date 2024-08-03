using System;
using System.Linq;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Experience;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class ShortcutCheatSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private SlowMotionService _slowMotionService;
        private AudioService _audioService;
        
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.R))
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    _data.ResetData();

            if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = Math.Abs(Time.timeScale - 1.0f) < 0.01 ? 0.0f : 1.0f;

            if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 2.0f;

            if (Input.GetKeyDown(KeyCode.Alpha3)) Time.timeScale = 3.0f;

            if (Input.GetKeyDown(KeyCode.U)) _ui.TriggerShowStateAllScreen();

            if (Input.GetKeyDown(KeyCode.W)) _ui.TriggerShowStateAllWorldUiScreen();

            if (Input.GetKeyDown(KeyCode.S)) _slowMotionService.StartSlowMotion(0.2f, 1.0f, 5.0f);

            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = 0; i < _data.PlayerData.TutrorialStates.Count; i++)
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = (TutorialStep)i;
                    //_ui.SetShowStateTutorialScreen(false, (StaticData.Enums.Tutorials)i);
                }

                for (int i = 0; i < _data.PlayerData.TutrorialStates.Count; i++)
                {
                    _world.NewEntity().Get<CompleteTutorialRequest>().TutorialStep = (TutorialStep)i;
                    //_ui.SetShowStateTutorialScreen(false, (StaticData.Enums.Tutorials)i);
                }

                _data.RuntimeData.CurrentGameState = GameState.GlobalMap;
            }


            if (Input.GetKeyDown(KeyCode.N))
            {
                //_ui.SetShowStateLevelCompleteScreen(true);
                _data.PlayerData.CurrentWarStepIndex++;
                _data.PlayerData.EventLevelIndex++;
                _world.NewEntity().Get<DisposeLevelRequest>();
                _world.NewEntity().Get<SpawnLevelRequest>();
            }

            if (Input.GetKeyDown(KeyCode.C))
                _playerFilter.GetEntity(0).Get<AddItemToInventoryRequest>().Value = _data.StaticData.ItemDatabase.First(x => (x.Id == "item_tnt_0"));

            if (Input.GetKeyDown(KeyCode.O))
                _audioService.ToggleSfx(false);
            
            if (Input.GetKeyDown(KeyCode.P))
                _audioService.ToggleMusic(false);

            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
                {
                    _data.PlayerData.Resources[type] = 999;
                }
            }
        }
    }
}