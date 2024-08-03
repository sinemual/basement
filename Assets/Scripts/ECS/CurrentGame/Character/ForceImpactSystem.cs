using Client.Data.Core;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class ForceImpactSystem : IEcsRunSystem
    {
        private SharedData _gameData;
        
        private EcsFilter<PushForceRequest> _damageFilter;

        public void Run()
        {
            foreach (var idx in _damageFilter)
            {
                ref var entity = ref _damageFilter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>();
                ref var pushForceRequest = ref entity.Get<PushForceRequest>();

                Transform initiator = pushForceRequest.Source.Get<GameObjectProvider>().Value.transform;
                Vector3 direction = entityGo.Value.transform.position - initiator.position;

                float pushForce = pushForceRequest.Force - entity.Get<Stats>().Value[StatType.Weight];
                if (pushForce < 0.0f)
                    pushForce = 0.1f;
                pushForce *= _gameData.BalanceData.PushForceCoef;
                direction *= pushForce;
                direction += Vector3.up * _gameData.BalanceData.PushForceUpCoef;

                entity.Get<AddForce>() = new AddForce
                {
                    Direction = direction,
                    ForceMode = ForceMode.Impulse,
                };
                
                entity.Del<PushForceRequest>();
            }
        }
    }
}