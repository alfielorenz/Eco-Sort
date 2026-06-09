using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public SceneController sceneController;
    public LevelManager levelManager; 

    private void Start()
    {
        
        if (levelManager == null) 
            levelManager = Object.FindFirstObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        switch (other.tag)
        {
            case "Bio":
            case "Non-Bio":
            case "Rec":
            case "Haz":
                TriggerGameOver();
                break;
        }
    }

  void TriggerGameOver()
{
    GameMemory.lastPlayedLevel = SceneManager.GetActiveScene().name;
    GameMemory.isEndlessMode = (ScoreManagerEndless.Instance != null);

    if (GameMemory.isEndlessMode && ScoreManagerEndless.Instance != null)
    {
        int finalScore = ScoreManagerEndless.Instance.currentScore;
        TimerEndless timerScript = Object.FindFirstObjectByType<TimerEndless>();
        float finalTime = (timerScript != null) ? timerScript.GetElapsedTime() : 0f;

        // 1. Fetch existing high scores
        int highScore = PlayerPrefs.GetInt("EndlessHighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("EndlessBestTime", 0f);

        // 2. Logic: Save if score is higher OR if score is same but time is lower
        bool isNewHighScore = (finalScore > highScore);
        bool isSameScoreButFaster = (finalScore == highScore && finalTime < bestTime);

        if (isNewHighScore || isSameScoreButFaster)
        {
            PlayerPrefs.SetInt("EndlessHighScore", finalScore);
            PlayerPrefs.SetFloat("EndlessBestTime", finalTime);
            PlayerPrefs.Save();
            Debug.Log("New Personal Best saved!");
        }

        // 3. Clear the *Current Session* keys
        PlayerPrefs.DeleteKey("EndlessSavedPoints");
        PlayerPrefs.DeleteKey("EndlessSavedTime");
        PlayerPrefs.Save();
    }
    else if (levelManager != null)
    {
        GameMemory.currentLevelNumber = levelManager.levelNumber;
        if (ScoreManager.Instance != null) 
            GameMemory.pointsEarned = ScoreManager.Instance.currentScore;
    }

    if (sceneController != null)
        sceneController.LoadSpecificScene("GameOver");
    else
        SceneManager.LoadScene("GameOver");
}
}