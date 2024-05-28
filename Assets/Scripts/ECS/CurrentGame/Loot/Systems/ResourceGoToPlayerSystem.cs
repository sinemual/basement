using Client.Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.Infrastructure.Services;
using DG.Tweening;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public class ResourceGoToPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private UserInterfaceEventBus _userInterfaceEventBus;
        private AudioService _audioService;
        private VibrationService _vibrationService;
        
        private EcsFilter<GoToPlayerRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var itemData = ref entity.Get<GoToPlayerRequest>().ItemData;
                var itemGo = entity.Get<GoToPlayerRequest>().ItemGo;

                if (itemData is ResourceItemData resData)
                {
                    var pos = itemGo.transform.position;
                    
                    itemGo.transform.GetChild(0).LookAt(_cameraService.GetCamera().transform.position, Vector3.up);

                    Vector3 jumpPos = new Vector3(pos.x + Random.insideUnitCircle.x * 0.5f, pos.y, pos.z + Random.insideUnitCircle.y * 0.5f);
                    Vector3 flyPos = _cameraService.GetCamera().ScreenToWorldPoint(_ui.ResourceScreen.GetFlyPoint(resData.Type).position);
                    float randomDelay = Random.Range(0.0f, 0.2f);
            
                    var sequence = DOTween.Sequence();
                    sequence.Append(itemGo.transform.DOJump(jumpPos, 0.35f, 1, 0.4f).SetEase(Ease.Linear));
                    sequence.AppendInterval(randomDelay);
                    sequence.Append(itemGo.transform.DOMove(flyPos, 0.5f).SetEase(Ease.InQuad));
                    sequence.Play();

                    sequence.OnComplete(() =>
                    {
                        _prefabFactory.Despawn(itemGo);
                        _audioService.Play(Sounds.PickupSound);
                        _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                        _ui.ResourceScreen.Impact(resData.Type);
                        
                        _world.NewEntity().Get<AddResourceRequest>() = new AddResourceRequest()
                        {
                            Type = resData.Type,
                            Amount = 1
                        };
                    });
                
                    entity.Del<GoToPlayerRequest>();
                }
            }
        }
    }
}