﻿using System.Linq;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitPlayerOnLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        
        private CameraService _cameraService;
        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<PlayerProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);

                entity.Get<Stats>().Value = new StatValue();
                entity.Get<BaseStats>().Value = new StatValue();
                foreach (var stat in _data.StaticData.CharactersData[CharacterType.Player].StartStats)
                {
                    entity.Get<Stats>().Value.Add(stat.Key, stat.Value);
                    entity.Get<BaseStats>().Value.Add(stat.Key, stat.Value);
                }

                entity.Get<Equipment>().Value = new EquipByType();
                foreach (var equip in _data.PlayerData.Equipment)
                {
                    foreach (var eq in _data.StaticData.AllEquip[equip.Key].Value)
                    {
                        if (eq.Level == equip.Value)
                        {
                            EquipItemData item = Object.Instantiate(eq);
                            entity.Get<Equipment>().Value.Add(equip.Key, item);
                        }
                    }
                }

                entity.Get<RecalculateStatsRequest>();
                
                _uiEventBus.PlayerDamage.OnInitPlayerHealthEvent(
                    entity.Get<Stats>().Value[StatType.FullHealth],
                    entity.Get<Stats>().Value[StatType.Health]);
                
                entity.Get<InitedMarker>();
            }
        }
    }
}