using System;
using System.Collections.Generic;
using Client.DevTools.MyTools;
using Data.Base;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Vector2 = System.Numerics.Vector2;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "GameData/AudioData", fileName = "AudioData")]
    public class AudioData : BaseDataSO
    {
        public AudioMixer musicMixer;
        public AudioMixer sfxMixer;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Sound> sounds;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Sound> soundtrackSounds;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Sound> ambientSounds;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Sound> miningSounds;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Sound> hitSounds;
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
        public List<Sound> attackSounds;
        
        public MiningSfxByType MiningByType;
        public HitSfxByType HitByType;
        public AttackSfxByType AttackByType;
        public AmbientSfxByLocationType AmbientByType;
        
        [Serializable]
        public class MiningSfxByType : SerializedDictionary<BlockType, string>
        {
        }
        
        [Serializable]
        public class HitSfxByType : SerializedDictionary<CharacterType, string>
        {
        }
        
        [Serializable]
        public class AttackSfxByType : SerializedDictionary<CharacterType, string>
        {
        }
        
        [Serializable]
        public class AmbientSfxByLocationType : SerializedDictionary<LocationType, string>
        {
        }
    }
}