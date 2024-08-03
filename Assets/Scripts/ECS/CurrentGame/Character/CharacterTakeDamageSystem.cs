using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Hit.Components;
using Client.ECS.CurrentGame.Hit.Systems;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Client.Infrastructure.Services;
using Data;
using DG.Tweening;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class CharacterTakeDamageSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private VibrationService _vibrationService;
        private AudioService _audioService;
        private AnalyticService _analyticService;

        private EcsFilter<CharacterProvider, HitRequest>.Exclude<DeadState> _requestfilter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _requestfilter)
            {
                ref var entity = ref _requestfilter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>();
                ref var character = ref entity.Get<CharacterProvider>();
                ref var hit = ref entity.Get<HitRequest>();
                ref var stats = ref entity.Get<Stats>();
                ref var ragdoll = ref entity.Get<RagdollProvider>();
                ref var hitterStats = ref hit.HitterEntity.Get<Stats>().Value;

                stats.Value[StatType.Health] -= hitterStats[StatType.Damage];

                entity.Get<PushForceRequest>() = new PushForceRequest
                {
                    Source = hit.HitterEntity,
                    Force = hit.HitterEntity.Get<Stats>().Value[StatType.PushForce]
                };

                if (!entity.Has<MeshMaterial>())
                    entity.Get<MeshMaterial>().Value = ragdoll.BodyParts[0].GetComponent<Renderer>().material;

                foreach (var bodyPart in ragdoll.BodyParts)
                {
                    bodyPart.GetComponent<Renderer>().material = entity.Get<MeshMaterial>().Value;
                    var mat = bodyPart.GetComponent<Renderer>().material;
                    bodyPart.GetComponent<Renderer>().material = _data.StaticData.HitMaterial;
                    var tempEntity = entity;
                    bodyPart.GetComponent<Renderer>().material.DOColor(new Color(1.0f, 1.0f, 1.0f), 0.05f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                        bodyPart.GetComponent<Renderer>().material = tempEntity.Get<MeshMaterial>().Value
                    );
                }

                if (stats.Value[StatType.Health] < 0)
                    entity.Get<DeadRequest>();

                entity.Get<HitEvent>();
                _vibrationService.Vibrate(NiceHaptic.PresetType.LightImpact);
                _audioService.Play(Sounds.HitSound);
                _audioService.Play(_data.AudioData.HitByType[character.Type]);
                _analyticService.LogEventWithParameter("enemy_hit", character.Type.ToString());
                CreateHitVFX(entityGo.Value.transform);
                
                if (_data.PlayerData.Equipment[PlayerEquipType.Bow] > 0)
                    CreateFightAnimView(entityGo.Value.transform);

                entity.Del<HitRequest>();
            }
        }

        private void CreateHitVFX(Transform vfxT)
        {
            EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.HitVfxPrefab, vfxT.position + (Vector3.up * 1.5f),
                vfxT.rotation);
            spawnEntity.Get<DespawnAtTimerRequest>();
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.DespawnVfxTime;
        }

        private void CreateFightAnimView(Transform spawnT)
        {
            EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.MiningAnimViewPrefabs[PlayerEquipType.Bow],
                spawnT.position + (Vector3.up * 0.5f),
                Quaternion.identity);

            var go = spawnEntity.Get<GameObjectProvider>().Value;
            go.transform.forward = _cameraService.GetCamera().transform.forward;
            spawnEntity.Get<MiningAnimViewProvider>().ToolImage.sprite =
                _playerFilter.GetEntity(0).Get<Equipment>().Value[PlayerEquipType.Bow].View.ItemSprite;

            spawnEntity.Get<DespawnAtTimerRequest>();
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.DespawnMiningViewTime;
        }
    }

    public struct MeshMaterial
    {
        public Material Value;
    }
}