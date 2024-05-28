using Client.Data.Core;
using Client.ECS.CurrentGame.Loot.Systems;
using Client.ECS.CurrentGame.Mining;
using Client.Infrastructure.Services;
using EPOOutline;
using Leopotam.Ecs;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Client
{
    public class DeathSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private VibrationService _vibrationService;

        private EcsFilter<DeadRequest>.Exclude<Timer<TimerToDeadDespawn>> _deathFilter;
        private EcsFilter<DeadState, TimerDoneEvent<TimerToDeadDespawn>>.Exclude<DespawnAtTimerRequest> _toDespawnFilter;

        public void Run()
        {
            foreach (var idx in _deathFilter)
            {
                ref var entity = ref _deathFilter.GetEntity(idx);
                ref var characterType = ref entity.Get<CharacterProvider>().Type;
                ref var characterGo = ref entity.Get<GameObjectProvider>().Value;
                ref var ragdoll = ref entity.Get<RagdollProvider>();

                _vibrationService.Vibrate(NiceHaptic.PresetType.MediumImpact);

                _world.NewEntity().Get<SpawnLootRequest>() = new SpawnLootRequest()
                {
                    SpawnPosition = characterGo.transform.position,
                    Loot = _data.StaticData.CharactersData[characterType].Loot
                };

                if (entity.Has<MeshMaterial>())
                    foreach (var bodyPart in ragdoll.BodyParts)
                    {
                        bodyPart.GetComponent<Renderer>().material = entity.Get<MeshMaterial>().Value;
                        bodyPart.gameObject.layer = _data.StaticData.GetIgnoreLayer;
                    }

                characterGo.GetComponent<Outlinable>().enabled = false;

                entity.Get<DeadState>();
                entity.Get<StopMovingRequest>();
                entity.Get<EnableRagdollRequest>();
                entity.Get<Timer<TimerToDeadDespawn>>().Value = _data.BalanceData.TimeToDespawnDeadBody;
                entity.Get<DeadEvent>().CharacterType = characterType;
                entity.Del<DeadRequest>();
            }

            foreach (var idx in _toDespawnFilter)
            {
                ref var entity = ref _toDespawnFilter.GetEntity(idx);
                entity.Get<DespawnAtTimerRequest>();
            }
        }
    }
}