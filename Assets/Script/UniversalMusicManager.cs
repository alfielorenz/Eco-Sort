using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class UniversalMusicManager : MonoBehaviour
{
    private static UniversalMusicManager instance;
    private AudioSource audioSource;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;

    private List<string> menuScenes = new List<string> { "MainMenu", "Campaign", "SelectMode" };
    
    private List<string> gameplayScenes = new List<string> { "Endless","Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9"
    , "Level 10", "Level 11", "Level 12", "Level 13", "Level 14", "Level 15" }; 

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
        SyncVolume();
        UpdateMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.name);
        SyncVolume();
    }

    private void SyncVolume()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        
        
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        audioSource.volume = savedVolume;
        
        Debug.Log("Music Volume Synced to: " + savedVolume);
    }

    void UpdateMusicForScene(string sceneName)
    {
        AudioClip selectedClip = null;

        if (menuScenes.Contains(sceneName)) selectedClip = menuMusic;
        else if (gameplayScenes.Contains(sceneName)) selectedClip = levelMusic;

        if (selectedClip != null && audioSource.clip != selectedClip)
        {
            audioSource.clip = selectedClip;
            audioSource.Play();
        }
    }
}