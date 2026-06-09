using UnityEngine;

public static class GameMemory
{
    public static string lastPlayedLevel;
    public static int pointsEarned; 
    public static int currentLevelNumber; 
    
    // ADD THIS FLAG
    public static bool isEndlessMode = false; 

// Current Session
    public static int endlessPoints;
    public static float endlessTime;

    // Best Records
    public static int endlessBestPoints;
    public static float endlessBestTime;
    public static void LoadEndlessProgress()
    {
        endlessPoints = PlayerPrefs.GetInt("EndlessSavedPoints", 0);
        endlessTime = PlayerPrefs.GetFloat("EndlessSavedTime", 0f);
        
        endlessBestPoints = PlayerPrefs.GetInt("EndlessHighScore", 0);
        endlessBestTime = PlayerPrefs.GetFloat("EndlessBestTime", 0f);
    }
}