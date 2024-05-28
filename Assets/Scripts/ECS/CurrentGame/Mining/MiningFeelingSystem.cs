using System;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Client.Infrastructure.Services;
using Data;
using DG.Tweening;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class MiningFeelingSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private VibrationService _vibrationService;
        private AudioService _audioService;
        private CameraService _cameraService;

        private EcsFilter<BlockProvider, MineEvent> _filter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var stats = ref entity.Get<Stats>();
                var block = entity.Get<BlockProvider>();

                block.Model.transform.DORewind();
                block.Model.transform
                    .DOPunchScale(new Vector3(0.15f, 0.15f, 0.15f), 0.1f, 2)
                    .SetEase(Ease.InBounce).SetLink(block.Model);

                CreateMiningVFX(block.Model.transform, block.Type);
                if (_data.PlayerData.Equipment[block.MineEquipType] > 0)
                    CreateMiningAnimView(block.Model.transform, block.MineEquipType);

                _vibrationService.Vibrate(stats.Value[StatType.Health] <= 0 ? NiceHaptic.PresetType.SoftImpact : NiceHaptic.PresetType.RigidImpact);
                _audioService.Play(stats.Value[StatType.Health] <= 0 ? Sounds.MineDoneSound : _data.AudioData.MiningByType[block.Type]);
            }
        }

        private void CreateMiningVFX(Transform vfxT, BlockType type)
        {
            EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.MiningVfxPrefabs[type], vfxT.position + (Vector3.up * 0.3f),
                vfxT.rotation);
            spawnEntity.Get<DespawnAtTimerRequest>();
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.DespawnVfxTime;
        }

        private void CreateMiningAnimView(Transform spawnT, EquipType type)
        {
            EcsEntity spawnEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.MiningAnimViewPrefabs[type], spawnT.position,
                Quaternion.identity);

            var go = spawnEntity.Get<GameObjectProvider>().Value;
            go.transform.forward = _cameraService.GetCamera().transform.forward;
            spawnEntity.Get<MiningAnimViewProvider>().ToolImage.sprite = _playerFilter.GetEntity(0).Get<Equipment>().Value[type].View.ItemSprite;

            spawnEntity.Get<DespawnAtTimerRequest>();
            spawnEntity.Get<Timer<DespawnTimer>>().Value = _data.BalanceData.DespawnMiningViewTime;
        }
    }
}