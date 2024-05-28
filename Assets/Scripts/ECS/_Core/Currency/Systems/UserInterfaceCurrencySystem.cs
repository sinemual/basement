using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class UserInterfaceCurrencySystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<EarnCurrencyEvent> _earnEventFilter;
        private EcsFilter<SpendCurrncyEvent> _spendEventFilter;

        public void Init()
        {
            //_ui.OpenSettingScreen.UpdateMoneyText(_data.PlayerData.Currency);
        }

        public void Run()
        {
            /*foreach (var idx in _earnEventFilter)
                _ui.OpenSettingScreen.UpdateMoneyText(_data.PlayerData.Currency);

            foreach (var idx in _spendEventFilter)
                _ui.OpenSettingScreen.UpdateMoneyText(_data.PlayerData.Currency);*/
        }
    }
}