using System;
using System.Collections.Generic;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;
using CameraType = Client.Data.CameraType;
using Random = UnityEngine.Random;

namespace Client
{
    public class CalculateExperienceLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<CurrentLevelTag>.Exclude<CalculatedTag> _levelFilter;

        private EcsFilter<BlockProvider> _blockFilter;
        private EcsFilter<SpawnPointDataProvider> _spawnFilter;

        public void Run()
        {
            foreach (var idx in _levelFilter)
            {
                ref var levelEntity = ref _levelFilter.GetEntity(idx);

                float allExp = 0;

                foreach (var block in _blockFilter)
                {
                    var blockType = _blockFilter.Get1(block).Type;
                    var blockLevel = _blockFilter.Get1(block).Level;
                    
                    foreach (var loot in _data.StaticData.BlocksData[blockType].Levels[blockLevel].Loot)
                    {
                        if (loot.ItemData is ExperienceData)
                        {
                            float blockExp = loot.Amount * (loot.Chance / 100.0f);
                            blockExp *= _data.StaticData.BlocksData[blockType].Levels[blockLevel].ExpMultiplierForLevelComplete;
                            allExp += blockExp;
                        }
                    }
                }


                foreach (var spawn in _spawnFilter)
                {
                    if (_spawnFilter.Get1(spawn).Prefab.TryGetComponent(out CharacterMonoProvider characterMonoProvider))
                    {
                        var charType = characterMonoProvider.Value.Type;
                        foreach (var loot in _data.StaticData.CharactersData[charType].Loot)
                        {
                            if (loot.ItemData is ExperienceData)
                            {
                                float charExp = loot.Amount * (loot.Chance / 100.0f);
                                allExp += charExp * (_spawnFilter.Get1(spawn).Chance / 100.0f);
                            }
                        }
                    }
                }

                _data.RuntimeData.NeededLevelExperience = (int)(allExp * _data.BalanceData.LevelDoneExperienceCoef);
                _data.RuntimeData.AllLevelExperience = (int)(allExp);
                _uiEventBus.Experience.OnGetExperience();
                levelEntity.Get<CalculatedTag>();
            }
        }
    }

    internal struct CalculatedTag : IEcsIgnoreInFilter
    {
    }
}