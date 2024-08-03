using Client.Data.Core;
using Client.Data.Equip;
using Data;
using Leopotam.Ecs;

namespace Client
{
    public class InitCharacterSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;

        private EcsFilter<CharacterProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var character = ref entity.Get<CharacterProvider>();

                entity.Get<Stats>().Value = new StatValue();
                entity.Get<BaseStats>().Value = new StatValue();
                foreach (var stat in _data.StaticData.CharactersData[character.Type].StartStats)
                {
                    entity.Get<Stats>().Value.Add(stat.Key, stat.Value);
                    entity.Get<BaseStats>().Value.Add(stat.Key, stat.Value);
                }

                //entity.Get<Equipment>().Value = new EquipByType();
                entity.Get<RecalculateStatsRequest>();
                entity.Get<InitedMarker>();
            }
        }
    }
}