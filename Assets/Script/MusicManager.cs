using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    
    public List<string> allowedScenes = new List<string> { "MainMenu", "Campaign", "LevelMode" };

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
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (!allowedScenes.Contains(scene.name))
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; 
            Destroy(gameObject);
        }
        else
        {
            
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}