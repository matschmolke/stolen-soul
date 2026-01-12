using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private static Dictionary<SoundType, int> activeSounds = new Dictionary<SoundType, int>();
    private static int maxCopiesPerSound = 2;

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

    public static void PlaySoundAtPosition(SoundType sound, Vector3 position, Transform listener, float maxDistance = 15f)
    {
        if (instance == null || listener == null) return;

        if (!activeSounds.ContainsKey(sound)) activeSounds[sound] = 0;
        if (activeSounds[sound] >= maxCopiesPerSound) return;

        activeSounds[sound]++;

        float distance = Vector3.Distance(listener.position, position);
        float volume = Mathf.Clamp01(1 - (distance / maxDistance));

        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);

        instance.StartCoroutine(instance.ResetActiveSound(sound, 1f));
    }

    private IEnumerator ResetActiveSound(SoundType sound, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (activeSounds.ContainsKey(sound))
            activeSounds[sound] = Mathf.Max(0, activeSounds[sound] - 1);
    }
}
