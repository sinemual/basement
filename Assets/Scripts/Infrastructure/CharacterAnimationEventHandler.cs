using Leopotam.Ecs;

namespace Client.Infrastructure.MonoBehaviour
{
    public class CharacterAnimationEventHandler : UnityEngine.MonoBehaviour
    {
        private EcsEntity characterEntity;
        
        private void Start()
        {
            if (transform.parent.TryGetComponent(out MonoEntity character))
                characterEntity = character.Entity;
        }

        public void Shoot()
        {
            characterEntity.Get<AnimationShootRequest>();
        }
    }

    public struct AnimationShootRequest : IEcsIgnoreInFilter
    {
    }
}
