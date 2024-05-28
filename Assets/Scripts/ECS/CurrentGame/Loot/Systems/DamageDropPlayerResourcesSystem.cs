using Client.Client;
using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public class DamageDropPlayerResourcesSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private AudioService _audioService;
        private UserInterfaceEventBus _userInterfaceEventBus;

        private EcsFilter<DropLootRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);

                var randomType = Random.Range(0, 4);
                var randomAmount = Random.Range(1, 4);
                
                for (int i = 0; i < randomAmount; i++)
                    if(_data.PlayerData.Resources[(ResourceType)randomType] >= 1)
                        DropResource((ResourceType)randomType);

                entity.Del<DropLootRequest>();
            }
        }

        private void DropResource(ResourceType resType)
        {
            _world.NewEntity().Get<SpendResourceRequest>() = new SpendResourceRequest()
            {
                Type = resType,
                Amount = 1
            };
            Vector3 dropPos = _cameraService.GetCamera().ScreenToWorldPoint(_ui.ResourceScreen.GetFlyPoint(resType).position);
            Vector3 dropFinalPos = _cameraService.GetCamera().ScreenToWorldPoint(_ui.ResourceScreen.GetFlyPoint(resType).position) + new Vector3(Random.Range(-25, -5), -25.0f, 0.0f);

            GameObject go = Object.Instantiate(_data.StaticData.ResourcesData[resType].View.DropItemPrefab, dropPos, Quaternion.identity);
            go.transform.GetChild(0).LookAt(_cameraService.GetCamera().transform.position + dropPos, Vector3.up);
            _audioService.Play(Sounds.DropSound);
            var sequence = DOTween.Sequence();
            sequence.Append(go.transform.DOJump(dropFinalPos, 0.1f, 1, 3.0f));
            sequence.Play();

            sequence.OnComplete(() => _prefabFactory.Despawn(go));
        }
    }
}