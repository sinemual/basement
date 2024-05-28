using Client.Data.Core;
using Data;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class HitTapProgressBarSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        private EcsFilter<HitEvent> _hitFilter;

        public void Run()
        {
            foreach (var idx in _hitFilter)
            {
                ref var entity = ref _hitFilter.GetEntity(idx);
                var stats = entity.Get<Stats>().Value;
                var entityGo = entity.Get<GameObjectProvider>().Value;

                CreateHitTapProgressWorldUI(ref entity, entityGo, stats[StatType.Health], stats[StatType.FullHealth]);
            }
        }

        private void CreateHitTapProgressWorldUI(ref EcsEntity entity, GameObject go, float health, float fullHealth)
        {
            EcsEntity spawnEntity = new EcsEntity();
            if (!entity.Has<TapProgressBar>())
            {
                Vector3 createPosition = go.transform.position + _cameraService.GetCamera().transform.position * 0.01f + Vector3.up * 1.5f;
                spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.HealthBarPrefab, createPosition, Quaternion.identity, go.transform);
            }
            else
            {
                spawnEntity = entity.Get<TapProgressBar>().Value;
            }

            ref var bar = ref spawnEntity.Get<HealthBarProvider>();

            for (int i = 0; i < bar.HealthImages.Count; i++)
            {
                bar.HealthImages[i].SetActive(false);
                bar.DamageImages[i].SetActive(false);
                bar.EmptyHealthImages[i].SetActive(false);
            }

            for (int i = 0; i < (int)fullHealth; i++)
            {
                bar.HealthImages[i].SetActive(true);
                bar.DamageImages[i].SetActive(true);
                bar.EmptyHealthImages[i].SetActive(true);
            }

            bar.HealthBarImage.DOFillAmount(health / fullHealth, 0.1f).OnComplete(() =>
            {
                spawnEntity.Get<HealthBarProvider>().DamageBarImage.DOFillAmount(health / fullHealth, 0.1f);
            });

            ref var spawnGo = ref spawnEntity.Get<GameObjectProvider>().Value;
            spawnGo.transform.position = go.transform.position + _cameraService.GetCamera().transform.position * 0.01f + Vector3.up * 2.0f;

            spawnGo.transform.DORewind();
            spawnGo.transform.DOPunchScale(Vector3.one * 0.0005f, 0.2f, 1).SetLink(spawnGo);
            spawnEntity.Get<IsBusyMarker>();
            spawnEntity.Get<TryDespawnBusyWorldUiAtTimerRequest>().DependedEntity = entity;
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.HideWorldUiTime;
            spawnGo.transform.LookAt(2 * spawnGo.transform.position - _cameraService.GetCamera().transform.position);

            entity.Get<TapProgressBar>().Value = spawnEntity;
        }
    }
}