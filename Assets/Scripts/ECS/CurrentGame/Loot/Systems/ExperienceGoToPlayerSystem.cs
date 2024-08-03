using Client.Data;
using Client.Data.Core;
using Client.Data.Equip;
using Client.ECS.CurrentGame.Experience;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Loot.Systems
{
    public class ExperienceGoToPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private UserInterfaceEventBus _userInterfaceEventBus;
        private AudioService _audioService;

        private EcsFilter<GoToPlayerRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var itemData = ref entity.Get<GoToPlayerRequest>().ItemData;
                var itemGo = entity.Get<GoToPlayerRequest>().ItemGo;

                if (itemData is ExperienceData expData)
                {
                    var pos = itemGo.transform.position;
                    var levelNum = _data.PlayerData.CurrentWarStepIndex;

                    itemGo.transform.GetChild(0).LookAt(_cameraService.GetCamera().transform.position, Vector3.up);

                    Vector3 jumpPos = new Vector3(pos.x + Random.insideUnitCircle.x * 0.5f, pos.y, pos.z + Random.insideUnitCircle.y * 0.5f);
                    Vector3 flyPos = _cameraService.GetCamera().ScreenToWorldPoint(_ui.OnLevelScreen.GetExperienceFlyPoint().position);
                    float randomDelay = Random.Range(0.0f, 0.2f);

                    var sequence = DOTween.Sequence();
                    sequence.Append(itemGo.transform.DOJump(jumpPos, 0.2f, 2, 0.3f).SetEase(Ease.Linear));
                    sequence.AppendInterval(randomDelay);
                    sequence.Append(itemGo.transform.DOMove(flyPos, 0.5f).SetEase(Ease.InQuad));
                    sequence.Play();

                    sequence.OnComplete(() =>
                    {
                        _prefabFactory.Despawn(itemGo);
                        _audioService.Play(Sounds.ExpSound);
                        if (levelNum == _data.PlayerData.CurrentWarStepIndex)
                            _world.NewEntity().Get<GetExperienceRequest>().Value = 1;
                    });

                    entity.Del<GoToPlayerRequest>();
                }
            }
        }
    }
}