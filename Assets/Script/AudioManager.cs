using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioData : ScriptableObject
{
    public List<Sound> sounds = new List<Sound>();
}

public enum SoundName
{
    // Define your sound names here
   BG1,
   BG2,
   LineFill,
   BoxFill,
   Button,
   GameOver,
}

[System.Serializable]
public class Sound
{
    public SoundName soundName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioSource BgAudioSource;
    public AudioData audioData;

    public static AudioManager instance;

    float _currentMusicVolume = 1;
    float _volume = 1;
    float _musicVolume = 1;
    float _soundVolume = 1;
    bool _muted = false;
    bool _soundMuted = false;
    bool _musicMuted = false;
    public float volume
    {
        get { return _volume; }
        set
        {
            _volume = value;
            BgAudioSource.volume = value * musicVolume * _currentMusicVolume;
        }
    }
    public float soundVolume
    {
        get { return _soundVolume * _volume; }
        set { _soundVolume = value; }
    }
    public float musicVolume
    {
        get { return _musicVolume * _volume; }
        set
        {
            _musicVolume = value;
            BgAudioSource.volume = _volume * value * _currentMusicVolume;
        }
    }
    public bool muted
    {
        get { return _muted; }
        set
        {
            audioSource.mute = value || soundMuted;
            BgAudioSource.mute = value || musicMuted;
            _muted = value;
        }
    }
    public bool soundMuted
    {
        get { return _soundMuted; }
        set
        {
            audioSource.mute = value || muted;
            _soundMuted = value;
        }
    }
    public bool musicMuted
    {
        get { return _musicMuted; }
        set
        {
            BgAudioSource.mute = value || muted;
            _musicMuted = value;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Play(SoundName soundName)
    {
        Sound sound = audioData.sounds.Find(sound => sound.soundName == soundName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

        audioSource.PlayOneShot(sound.clip);
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume * _soundVolume;
        audioSource.pitch = sound.pitch;
    }

    public void PlayInBackGround(SoundName soundName)
    {
        Sound sound = audioData.sounds.Find(sound => sound.soundName == soundName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        BgAudioSource.clip = sound.clip;
        BgAudioSource.Play();
        BgAudioSource.clip = sound.clip;
        BgAudioSource.volume = sound.volume * _musicVolume;
        BgAudioSource.pitch = sound.pitch;

        _currentMusicVolume = sound.volume;
    }
}
