using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource musicSource, ambientSource;
    private float musicVolume, sfxVolume;
    
    [SerializeField] private GameObject sfxPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }
    
    public void PlayAmbient(AudioClip ambient)
    {
        ambientSource.clip = ambient;
        ambientSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    public void PlaySFX(AudioClip sfxSound, Vector3 position)
    {
        var sfxPrefab = Instantiate(sfxSound, position, Quaternion.identity);
        var sfxAudioSource = sfxPrefab.GetComponent<AudioSource>();

        sfxAudioSource.clip = sfxSound;
        sfxAudioSource.volume = sfxVolume;
        sfxAudioSource.Play();
        Destroy(sfxPrefab, sfxSound.length);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        ambientSource.volume = volume;
    }
}