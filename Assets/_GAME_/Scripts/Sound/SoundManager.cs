using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("Scene music mapping")]
    [SerializeField] private SceneMusic[] sceneMusic;

    [SerializeField] private AudioClip[] soundList;

    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource typingSource;

    private SoundType? currentMusic;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {            
        int index = Array.FindIndex(
            sceneMusic,
            s => s.sceneName == scene.name
        );

        if (index != -1)
        {
            Debug.Log("Found music for!");
            PlayMusic(sceneMusic[index].music, 0.5f);
        }
        else
        {
            Debug.Log("No music assigned for scene: " + scene.name);
            return;
        }
    }
    private void PlayMusic(SoundType music, float volume = 1f)
    {
        if (currentMusic == music) return;

        currentMusic = music;

        audioSource.Stop();
        audioSource.clip = soundList[(int)music];
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.time = 0f;
        audioSource.Play();
    }
    
    public static void StopMusic()
    {
        if (instance == null) return;
        instance.audioSource.Stop();
    }

    public static void StartTyping(SoundType sound, float volume = 1f)
    {
        if (instance.typingSource.isPlaying) return;

        instance.typingSource.clip = instance.soundList[(int)sound];
        instance.typingSource.volume = volume;
        instance.typingSource.loop = true;
        instance.typingSource.Play();
    }

    public static void StopTyping()
    {
        if (instance.typingSource.isPlaying)
            instance.typingSource.Stop();
    }

}
