using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class RagdollSystem : IEcsRunSystem
    {
        private SharedData _data;
        
        private CameraService _cameraService;
        
        private EcsFilter<RagdollProvider>.Exclude<InitedRagdollMarker> _ragdollFilter;
        private EcsFilter<RagdollProvider, InitedRagdollMarker, EnableRagdollRequest> _requestFilter;

        public void Run()
        {
            foreach (var idx in _ragdollFilter)
            {
                ref var entity = ref _ragdollFilter.GetEntity(idx);
                ref var ragdoll = ref entity.Get<RagdollProvider>();

                foreach (var bodyPart in ragdoll.BodyParts)
                {
                    bodyPart.GetComponent<Collider>().enabled = false;
                    bodyPart.GetComponent<Collider>().isTrigger = true;
                    bodyPart.GetComponent<Rigidbody>().isKinematic = true;
                    bodyPart.GetComponent<Rigidbody>().detectCollisions = false;
                }

                entity.Get<InitedRagdollMarker>();
            }

            foreach (var idx in _requestFilter)
            {
                ref var entity = ref _requestFilter.GetEntity(idx);
                ref var ragdoll = ref entity.Get<RagdollProvider>();
                ref var go = ref entity.Get<GameObjectProvider>().Value;

                if (ragdoll.Animator)
                    ragdoll.Animator.enabled = false;
                if (ragdoll.MainCollider)
                    ragdoll.MainCollider.enabled = false;
                if (ragdoll.MainRigidbody)
                    ragdoll.MainRigidbody.isKinematic = true;

                var direciton =  go.transform.position - _cameraService.GetCamera().transform.position;
                var randomForce = Random.Range(-_data.BalanceData.DeadForceCoef, _data.BalanceData.DeadForceCoef);
                direciton = new Vector3(direciton.x, direciton.y + _data.BalanceData.PushForceUpCoef * 0.5f, direciton.z);
                var force = direciton.normalized * _data.BalanceData.DeadForceCoef;
                
                foreach (var bodyPart in ragdoll.BodyParts)
                {
                    bodyPart.GetComponent<Collider>().enabled = true;
                    bodyPart.GetComponent<Collider>().isTrigger = false;
                    bodyPart.GetComponent<Rigidbody>().isKinematic = false;
                    bodyPart.GetComponent<Rigidbody>().detectCollisions = true;

                    bodyPart.velocity = force;
                }
                entity.Del<EnableRagdollRequest>();
            }
        }
    }
}