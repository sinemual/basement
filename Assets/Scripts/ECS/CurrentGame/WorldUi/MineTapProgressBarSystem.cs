using Client.Data.Core;
using Client.Infrastructure.Services;
using Data;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class MineTapProgressBarSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        private EcsFilter<MineEvent> _mineFilter;

        public void Run()
        {
            foreach (var idx in _mineFilter)
            {
                ref var entiy = ref _mineFilter.GetEntity(idx);
                var stats = entiy.Get<Stats>().Value;
                var entityGo = entiy.Get<GameObjectProvider>().Value;
                float progress = stats[StatType.Health] / stats[StatType.FullHealth];

                CreateMineTapProgressWorldUI(ref entiy, entityGo, progress, stats[StatType.Health]);
            }
        }

        private void CreateMineTapProgressWorldUI(ref EcsEntity entity, GameObject go, float progress, float hp)
        {
            EcsEntity spawnEntity = new EcsEntity();
            if (!entity.Has<TapProgressBar>())
            {
                Vector3 createPosition = go.transform.position + _cameraService.GetCamera().transform.position * 0.05f;
                spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.TapProgressBarPrefab, createPosition, Quaternion.identity);
            }
            else
                spawnEntity = entity.Get<TapProgressBar>().Value;

            spawnEntity.Get<TapProgressBarProvider>().FillImage.fillAmount = hp;
            spawnEntity.Get<TapProgressBarProvider>().FillImage.DOFillAmount(progress, 0.1f);

            ref var spawnGo = ref spawnEntity.Get<GameObjectProvider>().Value;
            spawnGo.transform.DORewind();
            spawnGo.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f, 1).SetLink(spawnGo);
            spawnEntity.Get<IsBusyMarker>();
            spawnEntity.Get<TryDespawnBusyWorldUiAtTimerRequest>().DependedEntity = entity;
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.HideWorldUiTime;
            spawnGo.transform.LookAt(2 * spawnGo.transform.position - _cameraService.GetCamera().transform.position);

            entity.Get<TapProgressBar>().Value = spawnEntity;
        }
    }
}