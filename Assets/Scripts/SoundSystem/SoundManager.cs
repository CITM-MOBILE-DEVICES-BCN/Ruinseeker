using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con la UI

public enum SoundType
{
    BUTTON,
    CHECKPOINT,
    GEMPICKUP,
    KILLINGENEMY,
    MINEENEMY,
    BOOMERANGSHOT,
    JUMP,
    LEVELCOMPLETED,
    MUSIC,
    WALLBUMP
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private Slider effectsVolumeSlider; // Referencia al slider de efectos
    [SerializeField] private Slider musicVolumeSlider;   // Referencia al slider de música
    private static SoundManager instance;
    private AudioSource effectsAudioSource;
    private AudioSource musicAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        musicAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        effectsAudioSource = GetComponent<AudioSource>();

        if (effectsVolumeSlider != null)
        {
            effectsVolumeSlider.onValueChanged.AddListener(SetEffectsVolume);
            effectsVolumeSlider.value = effectsAudioSource.volume; 
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            musicVolumeSlider.value = musicAudioSource.volume;
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        if (instance != null && instance.effectsAudioSource != null && instance.soundList != null)
        {
            instance.effectsAudioSource.PlayOneShot(instance.soundList[(int)sound], volume * instance.effectsAudioSource.volume);
        }
    }

    public static void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (instance != null && instance.musicAudioSource != null)
        {
            instance.musicAudioSource.clip = musicClip;
            instance.musicAudioSource.loop = loop;
            instance.musicAudioSource.Play();
        }
    }

    public void SetEffectsVolume(float volume)
    {

        AudioListener.volume = effectsVolumeSlider.value;
    }

    public void SetMusicVolume(float volume)
    {
        AudioListener.volume = musicVolumeSlider.value;
    }
}
