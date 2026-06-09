using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelMusicManager : MonoBehaviour
{
    private static LevelMusicManager instance;

    
    public List<string> allowedScenes = new List<string> { "Level 1","Level 2","Level 3","Level 4","Level 5",
    "Level 6","Level 7","Level 8","Level 9","Level 10","Level 11","Level 12","Level 13","Level 14","Level 15" };

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
    }
}