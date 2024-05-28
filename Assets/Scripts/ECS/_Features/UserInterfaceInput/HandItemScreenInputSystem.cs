using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.ECS.CurrentGame.PlayerEquipment;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client
{
    public class HandItemScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private AudioService _audioService;
        private UserInterfaceEventBus _userInterfaceEventBus;
        private VibrationService _vibrationService;

        private EcsFilter<PlayerProvider> _playerFilter;

        public void Init()
        {
            _userInterfaceEventBus.CurrentHandItemScreen.UseItemButtonTap += () =>
            {
                if (_data.RuntimeData.CurrentGameState != GameState.OnLevel)
                    return;

                ref var entity = ref _playerFilter.GetEntity(0);
                if (entity.Has<HandItem>())
                {
                    _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                    _world.NewEntity().Get<CreateDragItemRequest>().Value = entity.Get<HandItem>().Data;
                    _world.NewEntity().Get<RemoveItemFromInventoryRequest>().Value = entity.Get<HandItem>().Data;
                    entity.Get<DragHandItemState>();
                }
            };
        }
    }
}