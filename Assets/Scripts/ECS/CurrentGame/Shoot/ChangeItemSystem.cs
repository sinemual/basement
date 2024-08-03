using Client.Data.Core;
using Client.Data.Equip;
using Leopotam.Ecs;

namespace Client
{
    public class ChangeItemSystem :IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;
        private UserInterfaceEventBus _uiEventBus;
        private PrefabFactory _prefabFactory;

        public void Init()
        {
            _uiEventBus.ChangeItemScreen.ChooseItemButtonTap += () =>
            {
                _data.RuntimeData.currentPlayerEquipMode = _data.RuntimeData.currentPlayerEquipMode == PlayerEquipType.Bow ? PlayerEquipType.Pickaxe : PlayerEquipType.Bow;
                _uiEventBus.ChangeItemScreen.OnChangeItem(_data.RuntimeData.currentPlayerEquipMode);
            };
        }
    }
}