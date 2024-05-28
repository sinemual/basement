using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Hit.Components;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.Infrastructure.MonoBehaviour;
using Client.Infrastructure.Services;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class EnemyShootPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        private VibrationService _vibrationService;
        private AudioService _audioService;
        
        private EcsFilter<EnemyShootProvider, AnimationShootRequest> _shootFilter;
        private EcsFilter<PlayerProvider> _playerFilter;

        public void Run()
        {
            foreach (var idx in _shootFilter)
            {
                ref var entity = ref _shootFilter.GetEntity(idx);
                ref var enemy = ref entity.Get<EnemyProvider>();
                ref var entityCharacter = ref entity.Get<CharacterProvider>();
                ref var shooter = ref entity.Get<EnemyShootProvider>();
                
                entity.Del<AnimationShootRequest>();
                var playerEntity = _playerFilter.GetEntity(0);
                var playerGo = playerEntity.Get<GameObjectProvider>().Value;
                var playerShotPoint = playerEntity.Get<PlayerProvider>().ShotPoint;
                var direction = playerGo.transform.position - enemy.Head.transform.position;
                var rotation = Quaternion.LookRotation(direction, Vector3.up);
                
                _audioService.Play(_data.AudioData.AttackByType[entityCharacter.Type]);
                
                EcsEntity arrowEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.ShotPrefabs[entityCharacter.Type], shooter.ShotPoint.position, rotation);
                var arrowGo = arrowEntity.Get<GameObjectProvider>().Value;

                var randomShotChance = Random.Range(0, 100);
                var randomXRot = -180 /*+ Random.Range(-15, 15)*/;
                var randomYRot = 220 + Random.Range(-15, 15);
                
                if (_data.BalanceData.ArrowShotChance > randomShotChance)
                {
                    var sequence = DOTween.Sequence();
                    sequence.Append(arrowGo.transform.DOJump(playerShotPoint.position, 3.0f, 1, _data.BalanceData.ArrowSpeed)).SetEase(Ease.InCirc);
                    sequence.Join(arrowGo.transform.DOScale(Vector3.one * 15.0f ,_data.BalanceData.ArrowSpeed)).SetEase(Ease.InCirc);
                    sequence.Append(arrowGo.transform.DORotate(new Vector3(randomXRot, randomYRot, -240) ,0.05f));
                    sequence.Play();

                    var hitterEntity = entity;
                    sequence.OnComplete(() =>
                    {
                        arrowGo.transform.SetParent(playerShotPoint);
                        arrowGo.transform.localRotation = Quaternion.Euler(new Vector3(30, 175 + Random.Range(-15, 15), 240));
                        playerEntity.Get<HitRequest>().HitterEntity = hitterEntity;
                        arrowEntity.Get<DespawnAtTimerRequest>();
                        arrowEntity.Get<Timer<DespawnTimer>>().Value = 10.0f;
                    });
                }
                else
                {
                    var sequence = DOTween.Sequence();
                    sequence.Append(arrowGo.transform.DOJump(playerGo.transform.position + Random.onUnitSphere * 10.0f, 2.0f, 1, _data.BalanceData.ArrowSpeed)).SetEase(Ease.InCirc);
                    sequence.Join(arrowGo.transform.DOScale(Vector3.one  * 15.0f ,_data.BalanceData.ArrowSpeed)).SetEase(Ease.InCirc);
                    sequence.Play();

                    sequence.OnComplete(() => _prefabFactory.Despawn(ref arrowEntity));
                }

                entity.Del<ShootRequest>();
            }
        }
    }
}