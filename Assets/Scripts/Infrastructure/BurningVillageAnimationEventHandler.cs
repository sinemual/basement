using Leopotam.Ecs;
using UnityEngine;

namespace Client.Infrastructure.MonoBehaviour
{
    public class BurningVillageAnimationEventHandler : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Sound burningSound;
        [SerializeField] private Sound flySound;
        private EcsEntity entity;
        
        private void Start()
        {
            if (transform.TryGetComponent(out MonoEntity monoEntity))
                entity = monoEntity.Entity;
            
            burningSound.source = gameObject.AddComponent<AudioSource>();
            burningSound.source.clip = burningSound.clip;
            burningSound.source.loop = burningSound.loop;
            burningSound.source.outputAudioMixerGroup = burningSound.mixer;
            burningSound.source.volume = burningSound.volume * (1f + Random.Range(-burningSound.volumeVariance / 2f, burningSound.volumeVariance / 2f));
            burningSound.source.pitch = burningSound.pitch * (1f + Random.Range(-burningSound.pitchVariance / 2f, burningSound.pitchVariance / 2f));
            
            flySound.source = gameObject.AddComponent<AudioSource>();
            flySound.source.clip = flySound.clip;
            flySound.source.loop = flySound.loop;
            flySound.source.outputAudioMixerGroup = flySound.mixer;
            flySound.source.volume = flySound.volume * (1f + Random.Range(-flySound.volumeVariance / 2f, flySound.volumeVariance / 2f));
            flySound.source.pitch = flySound.pitch * (1f + Random.Range(-flySound.pitchVariance / 2f, flySound.pitchVariance / 2f));
        }

        //calls from animation
        public void EndBurningVillageSceneEvent() => entity.Get<EndBurningVillageSceneEvent>();
        public void FlySoundPlay() => flySound.source.Play();
        public void BurningSoundPlay() => burningSound.source.Play();
    }

    public struct EndBurningVillageSceneEvent : IEcsIgnoreInFilter
    {
    }
}
