using System.Linq;
using Client.Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client
{
    public class CraftSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;
        
        private AudioService _audioService;
        private VibrationService _vibrationService;
        
        private UserInterfaceEventBus _userInterfaceEventBus;

        public void Init()
        {
            _userInterfaceEventBus.CraftScreen.CraftButtonTap += (CraftRecipeData craftedItemData) =>
            {
                _world.NewEntity().Get<ItemCraftedEvent>().Value = Object.Instantiate(craftedItemData.GettedItem);
                //_data.PlayerData.CraftedItem.Add(craftedItemData.GettedItem.Id);
                _audioService.Play(Sounds.CraftSound);
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                foreach (var neededItem in craftedItemData.NeededItems)
                    if (neededItem.ItemData is ResourceItemData res)
                    {
                        _world.NewEntity().Get<SpendResourceRequest>() = new SpendResourceRequest()
                        {
                            Type = res.Type,
                            Amount = neededItem.Amount
                        };
                    }
            };
        }
    }
}