using Client.Data.Core;
using Leopotam.Ecs;

namespace Client.ECS.CurrentGame.Mining
{
    public class ShowTntPickaxeBoosterOfferSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        
        private EcsFilter<LevelInitEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                if(_data.PlayerData.EventLevelIndex > 10 && _data.RuntimeData.NeededLevelExperience > 30 && _data.PlayerData.EventLevelIndex % 2 == 0)
                    _ui.OpenTntPickaxeBoosterScreen.SetShowState(true);
            }
        }
    }
}