using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup mixer;
    
    [FoldoutGroup("Settings")] [Range(0f, 1f)] public float volume = .75f;
    [FoldoutGroup("Settings")] [Range(0f, 1f)] public float volumeVariance = .1f;
    [FoldoutGroup("Settings")] [Range(.1f, 3f)] public float pitch = 1f;
    [FoldoutGroup("Settings")] [Range(0f, 1f)] public float pitchVariance = .1f;
    [FoldoutGroup("Settings")] public bool loop;
    
    [HideInInspector] public AudioSource source;
}