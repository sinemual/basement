using System;
using System.Collections.Generic;
using Client.Data;
using Client.Data.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AudioService
{
    private AudioData _audioData;
    private SharedData _data;
    private GameObject _audioSourcesContainer;

    private List<Sound> allSounds;
    public AudioService(SharedData data)
    {
        _data = data;
        _audioData = data.AudioData;
        _audioSourcesContainer = Object.Instantiate(_data.StaticData.PrefabData.EmptyPrefab);
        _audioSourcesContainer.gameObject.name = "AudioSourcesContainer";

        allSounds = new List<Sound>();
        allSounds.AddRange(_audioData.sounds);
        allSounds.AddRange(_audioData.soundtrackSounds);
        allSounds.AddRange(_audioData.miningSounds);
        allSounds.AddRange(_audioData.ambientSounds);
        allSounds.AddRange(_audioData.hitSounds);
        allSounds.AddRange(_audioData.attackSounds);
        Init();
    }

    private void Init()
    {
        foreach (var s in allSounds)
        {
            s.source = _audioSourcesContainer.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixer;
        }
    }

    public void Play(string soundName)
    {
        var sound = allSounds.Find(item => item.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        sound.source.volume = sound.volume * (1f + Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f));
        sound.source.pitch = sound.pitch * (1f + Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

        sound.source.Play();
    }
    
    public void StopAllAmbient()
    {
        foreach (var s in allSounds)
        {
            if(_audioData.ambientSounds.Contains(s))
                s.source.Stop();
        }
    }

    public void Stop(string soundName)
    {
        var sound = allSounds.Find(item => item.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        sound.source.Stop();
    }

    public void AllStop()
    {
        foreach (var sound in allSounds)
            if (sound.source != null)
                sound.source.Stop();
    }

    public void ToggleAudio(bool on)
    {
        _audioData.musicMixer.SetFloat("Volume", on? -5f : -80f);
        _audioData.sfxMixer.SetFloat("Volume", on? -5f : -80f);
    }
    
    public void ToggleSfx(bool on) => _audioData.sfxMixer.SetFloat("Volume", on? -5f : -80f);
    public void ToggleMusic(bool on) => _audioData.musicMixer.SetFloat("Volume", on? -5f : -80f);
}