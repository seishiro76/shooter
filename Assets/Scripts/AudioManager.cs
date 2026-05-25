using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Serializable]
    private class SoundData
    {
        public SoundType type;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;
    }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Sounds")]
    [SerializeField] private List<SoundData> sounds = new List<SoundData>();

    private Dictionary<SoundType, SoundData> soundMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }

        if (sfxSource != null)
        {
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.spatialBlend = 0f;
        }

        soundMap = new Dictionary<SoundType, SoundData>();

        foreach (SoundData sound in sounds)
        {
            if (!soundMap.ContainsKey(sound.type))
            {
                soundMap.Add(sound.type, sound);
            }
        }
    }

    public void PlaySFX(SoundType type)
    {
        if (sfxSource == null || soundMap == null)
        {
            return;
        }

        if (!soundMap.TryGetValue(type, out SoundData sound))
        {
            return;
        }

        if (sound.clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }
}