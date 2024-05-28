using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class CalculateCurrencySystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private EcsFilter<AddCurrencyRequest> _addFilter;
        private EcsFilter<SubtractCurrencyRequest> _subtractFilter;

        public void Run()
        {
            foreach (var idx in _addFilter)
            {
                ref var entity = ref _addFilter.GetEntity(idx);
                _data.PlayerData.Currency += _addFilter.Get1(idx).Value;

                _world.NewEntity().Get<EarnCurrencyEvent>().Value = entity.Get<AddCurrencyRequest>().Value;
                entity.Del<AddCurrencyRequest>();
            }

            foreach (var idx in _subtractFilter)
            {
                ref var entity = ref _subtractFilter.GetEntity(idx);
                _data.PlayerData.Currency -= _subtractFilter.Get1(idx).Value;
                if (_data.PlayerData.Currency < 0)
                    _data.PlayerData.Currency = 0;

                _world.NewEntity().Get<SpendCurrncyEvent>().Value = entity.Get<SubtractCurrencyRequest>().Value;
                entity.Del<SubtractCurrencyRequest>();
            }
        }
    }
}