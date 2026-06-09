using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LevelInitializer : MonoBehaviour
{
    public int levelNumber = 1; // Assign this in the Inspector (e.g., 2 for Level 2)

    void Awake()
    {
        // This is the CRITICAL line that makes the other scripts work
        GameMemory.currentLevelNumber = levelNumber;
        Debug.Log("GameMemory set to level: " + GameMemory.currentLevelNumber);
    }
}
public class LevelManager : MonoBehaviour
{
    [Header("Level Configuration")]
    public int levelNumber = 1; 
    public float timeRemaining = 120f;
    public GameObject winPopUpPanel;
    public GameObject countdownPanel;
    
    private int score; 
    private bool isGameOver = false;
    private bool waitingForClick = false;
    [HideInInspector] public bool gameStarted = false; 


    void Start() 
    { 
        if (winPopUpPanel != null) winPopUpPanel.SetActive(false); 
        if (countdownPanel != null) countdownPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void StartLevel() 
    { 
        gameStarted = true; 
        Time.timeScale = 1f; 
        if (countdownPanel != null) countdownPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
        {
            if (waitingForClick && Mouse.current.leftButton.wasPressedThisFrame) GoToRewardScene();
            return;
        }

        if (gameStarted)
        {
            if (ScoreManager.Instance != null) score = ScoreManager.Instance.currentScore;
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (score >= 100) WinLevel();
            }
            else
            {
                if (score >= 30) WinLevel();
                else LoseLevel();
            }
        }
    }

    void WinLevel()
    {
        isGameOver = true;
        if (winPopUpPanel != null)
        {
            winPopUpPanel.SetActive(true);
            Time.timeScale = 0f; 
            waitingForClick = true;
        }
    }

    void GoToRewardScene()
{
        Time.timeScale = 1f;
        GameMemory.pointsEarned = score;
        // THIS LINE IS CRITICAL:
        GameMemory.lastPlayedLevel = SceneManager.GetActiveScene().name; 
        GameMemory.currentLevelNumber = levelNumber;
        SceneManager.LoadScene("NextLevel");
}

    void LoseLevel() { isGameOver = true; SceneManager.LoadScene("GameOver"); }
}