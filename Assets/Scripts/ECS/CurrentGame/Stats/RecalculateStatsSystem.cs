using System;
using Client.Data.Core;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class RecalculateStatsSystem : IEcsRunSystem
    {
        private SharedData _data;
        private CameraService _cameraService;

        private EcsFilter<Stats, RecalculateStatsRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                
                foreach (StatType stat in (StatType[])Enum.GetValues(typeof(StatType)))
                    if (entity.Get<Stats>().Value.ContainsKey(stat))
                        entity.Get<Stats>().Value[stat] = _data.RuntimeData.CalculateStat(ref entity, stat);

                /*foreach (var stat in entity.Get<Stats>().Value)
                    Debug.Log($"stat: {stat.Key} = {stat.Value}");*/

                entity.Del<RecalculateStatsRequest>();
            }
        }
    }
}