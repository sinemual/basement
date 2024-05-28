using Client.Data.Core;
using Client.Data.Equip;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public class UiUsableItemGoToPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        private EcsFilter<UiGoToPlayerRequest> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                var itemData = entity.Get<UiGoToPlayerRequest>().ItemData;
                var itemGo = entity.Get<UiGoToPlayerRequest>().ItemGo;

                if (itemData is IUsable)
                {
                    var pos = itemGo.transform.position;
                    
                    Vector3 jumpPos = new Vector3(pos.x + Random.insideUnitCircle.x * 255.5f, pos.y + Random.insideUnitCircle.y * 255.5f, pos.z );
                    Vector3 flyPos = _ui.HandItemScreen.GetFlyPoint().position;
                    float randomDelay = Random.Range(0.0f, 0.2f);
            
                    var sequence = DOTween.Sequence();
                    sequence.Append(itemGo.transform.DOJump(jumpPos, 0.35f, 1, 0.4f).SetEase(Ease.Linear));
                    sequence.AppendInterval(randomDelay);
                    sequence.Append(itemGo.transform.DOMove(flyPos, 0.5f).SetEase(Ease.InQuad));
                    sequence.Play();

                    sequence.OnComplete(() =>
                    {
                        _prefabFactory.Despawn(itemGo);
                        _playerFilter.GetEntity(0).Get<AddItemToInventoryRequest>().Value = itemData;
                    });
                
                    entity.Del<UiGoToPlayerRequest>();
                }
            }
        }
    }
}