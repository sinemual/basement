using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client
{
    public class ExplosionSystem : IEcsRunSystem
    {
        private SharedData _data;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private VibrationService _vibrationService;
        private AudioService _audioService;
        
        private EcsFilter<ExplosionSourceProvider, ExplosionRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var explosion = ref entity.Get<ExplosionRequest>();
                
                Vector3 explosionPoint = entityGo.transform.position;
                Collider[] hits = new Collider[32];
                Physics.OverlapSphereNonAlloc(explosionPoint, explosion.Radius, hits);

                foreach (Collider hit in hits)
                    if (hit && hit.gameObject.TryGetComponent(out MonoEntity monoEntity))
                        monoEntity.Entity.Get<ExplosionHitRequest>().ExplosionSourceEntity = entity;

                CreateExplosionVFX(explosionPoint);
                _cameraService.Shake();
                _vibrationService.Vibrate(NiceHaptic.PresetType.HeavyImpact);
                _audioService.Play(Sounds.ExplodeSound);
                
                entity.Get<GameObjectProvider>().Value.SetActive(false);
                entity.Get<DespawnAtTimerRequest>();
                entity.Get<Timer<DespawnTimer>>().Value = 1.0f; // 2
                
                entity.Del<ExplosionRequest>();
            }
        }
        
        private void CreateExplosionVFX(Vector3 vfxT)
        {
            EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.ExplosionVfxPrefab, vfxT,
                Quaternion.identity);
            spawnEntity.Get<DespawnAtTimerRequest>();
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.DespawnVfxTime;
        }
    }
}