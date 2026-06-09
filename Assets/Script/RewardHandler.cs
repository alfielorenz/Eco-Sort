using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RewardHandler : MonoBehaviour
{
    public GameObject coinVisual;   
    public TextMeshProUGUI coinAmountText;   
    public TextMeshProUGUI pointsEarnedText; 

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "LevelMode") 
        {
            if (coinVisual != null) coinVisual.SetActive(false);
            return;
        }

        string levelName = !string.IsNullOrEmpty(GameMemory.lastPlayedLevel) ? GameMemory.lastPlayedLevel : "EditorTest";
        int points = GameMemory.pointsEarned;

        if (pointsEarnedText != null) pointsEarnedText.text = points.ToString() + " PTS";

        
        SaveLevelProgress(levelName, points);

        
        UnlockNextLevel(GameMemory.currentLevelNumber);
        HandleCompletionReward(levelName, points);
    }

    void SaveLevelProgress(string levelName, int currentScore)
    {
        
        int currentStars = 0;
        if (currentScore >= 100) currentStars = 3;
        else if (currentScore >= 70) currentStars = 2; 
        else if (currentScore >= 30) currentStars = 1;

        // Save for WinDisplay (1, 2, or 3)
        PlayerPrefs.SetInt("LastStarsEarned", currentStars);

        // Save permanent progress for the Map
        int savedHighScore = PlayerPrefs.GetInt(levelName + "_HighScore", 0);
        int savedStars = PlayerPrefs.GetInt(levelName + "_Stars", 0);

        if (currentScore > savedHighScore)
            PlayerPrefs.SetInt(levelName + "_HighScore", currentScore);

        if (currentStars > savedStars)
            PlayerPrefs.SetInt(levelName + "_Stars", currentStars);

        PlayerPrefs.Save();
    }

    void UnlockNextLevel(int completedLevelNum)
    {
        int currentReached = PlayerPrefs.GetInt("ReachedLevel", 1);
        if (completedLevelNum == currentReached)
        {
            PlayerPrefs.SetInt("ReachedLevel", currentReached + 1);
            PlayerPrefs.Save();
        }
    }

    public void HandleCompletionReward(string level, int points)
    {
        string saveKey = "LevelCleared_" + level;
        if (PlayerPrefs.GetInt(saveKey, 0) == 0)
        {
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
            if (CoinManager.Instance != null) CoinManager.Instance.AddRewards(20);
            if (coinAmountText != null) coinAmountText.text = "+20";
        }
        else
        {
            if (coinAmountText != null) coinAmountText.text = "+0";
        }
    }
}