using System.Linq;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Hit.Components;
using Client.Infrastructure.Services;
using Data;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class HitChestSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;
        
        private CameraService _cameraService;
        private PrefabFactory _prefabFactory;
        private AnalyticService _analyticService;
        private AudioService _audioService;
        
        private EcsFilter<ChestProvider, HitRequest> _filter;
        private EcsFilter<LevelProvider, CurrentLevelTag> _levelFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                var go = entity.Get<GameObjectProvider>().Value;
                
                ChestItemData itemData = _data.StaticData.ChestsData[_data.RuntimeData.CurrentLocationType];

                go.GetComponent<Collider>().enabled = false;
                _audioService.Play(Sounds.ChestFly);
                
                Vector3 flyPos = _cameraService.GetCamera().ScreenToWorldPoint(_ui.ChestScreen.GetFlyPoint().position) - Vector3.up;
                
                var sequence = DOTween.Sequence();
                sequence.Append(go.transform.DOMove(flyPos, 1.0f).SetEase(Ease.InQuad));
                sequence.Join(go.transform.DOScale(Vector3.one * 2.2f, 1.0f).SetEase(Ease.InQuad));
                sequence.Join(go.transform.DOLookAt(_cameraService.GetCamera().transform.position + Vector3.left, 1.0f).SetEase(Ease.InQuad));
                sequence.Play();

                sequence.OnComplete(() =>
                {
                    _prefabFactory.Despawn(go);
                    _ui.ChestScreen.SetChestData(itemData);
                    _ui.ChestScreen.SetShowState(true);
                    _world.NewEntity().Get<ChestFoundEvent>();
                    
                });
                
                entity.Del<HitRequest>();
            }
        }
    }

    public struct TimerToFly
    {
        public float Value;
    }
}