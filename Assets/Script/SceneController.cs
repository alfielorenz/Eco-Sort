using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    [Header("Pause Settings")]
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    [Header("End Game UI")]
    public GameObject noMoreLevelsPanel; // Assign this in the Inspector

    // Track if the popup is currently visible
    private bool popupActive = false;

    public void LoadNextLevel()
{
        string lastLevel = GameMemory.lastPlayedLevel;
        if (string.IsNullOrEmpty(lastLevel)) lastLevel = "Level 1"; 

        string numberPart = System.Text.RegularExpressions.Regex.Match(lastLevel, @"\d+").Value;

        // 1. Declare the variable OUTSIDE the if-block
        int levelNum = 0; 

        // 2. Use the declared variable in the TryParse
        if (int.TryParse(numberPart, out levelNum)) 
        {
            // Now 'levelNum' is accessible anywhere inside LoadNextLevel()
            int nextLevelNumber = levelNum + 1;
            string nextLevelName = "Level " + nextLevelNumber;
            
            Debug.Log("Trying to load: " + nextLevelName);

            // Update the global memory for the next level
            GameMemory.currentLevelNumber = nextLevelNumber;

            if (DoesSceneExist(nextLevelName))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                ShowWinMessage(); 
            }
        }
        else
        {
            ShowWinMessage();
        }
}

// Helper to verify the scene is actually in your Build Settings
    bool DoesSceneExist(string name)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Contains(name)) return true;
        }
        return false;
    }
    private void ShowWinMessage()
    {
        if (noMoreLevelsPanel != null)
        {
            noMoreLevelsPanel.SetActive(true);
            noMoreLevelsPanel.transform.SetAsLastSibling(); // Ensure it's on top
            popupActive = true; // Activate the click-to-close logic
        }
    }
    // --- UPDATED LOADING LOGIC ---
    public void LoadSpecificScene(string sceneName)
    {
        string targetScene = sceneName;

        if (targetScene != "GameOver" && ScoreManagerEndless.Instance != null)
        {
            GameMemory.endlessPoints = ScoreManagerEndless.Instance.currentScore;
            PlayerPrefs.SetInt("EndlessSavedPoints", GameMemory.endlessPoints);
            
            TimerEndless timerScript = Object.FindFirstObjectByType<TimerEndless>();
            if (timerScript != null)
            {
                GameMemory.endlessTime = timerScript.GetElapsedTime();
                PlayerPrefs.SetFloat("EndlessSavedTime", GameMemory.endlessTime);
            }
            PlayerPrefs.Save();
        }
        else if (ScoreManager.Instance != null)
        {
            GameMemory.pointsEarned = ScoreManager.Instance.currentScore;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(targetScene);
    }

    // --- RETRY LOGIC ---
    public void RetryLastLevel()
    {
        // 1. Clear Level Mode data
        PlayerPrefs.DeleteKey("CampaignSavedPoints");
        PlayerPrefs.Save();

        // 2. Reset time and reload the last level
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(GameMemory.lastPlayedLevel))
        {
            SceneManager.LoadScene(GameMemory.lastPlayedLevel);
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    // --- RESTART LOGIC ---
   public void RestartEndless()
    {
        ReturnConfirmation confirmation = Object.FindFirstObjectByType<ReturnConfirmation>();
        if (confirmation != null)
        {
            // Pass the string argument here!
            confirmation.OpenPopup("RestartEndless");
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // --- SAVE AND QUIT ---
    public void SaveAndQuitEndless(string mainMenuSceneName)
    {
        if (ScoreManagerEndless.Instance != null)
        {
            PlayerPrefs.SetInt("EndlessSavedPoints", ScoreManagerEndless.Instance.currentScore);
            TimerEndless timerScript = Object.FindFirstObjectByType<TimerEndless>();
            if (timerScript != null)
                PlayerPrefs.SetFloat("EndlessSavedTime", timerScript.GetElapsedTime());
        }
        else if (ScoreManager.Instance != null)
        {
            PlayerPrefs.SetInt("CampaignSavedPoints", ScoreManager.Instance.currentScore);
        }

        PlayerPrefs.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // --- PAUSE LOGIC ---
    public void TogglePause()
    {
        LevelManager levelManager = Object.FindFirstObjectByType<LevelManager>();
        EndlessManager endlessManager = Object.FindFirstObjectByType<EndlessManager>();

        // Optional check if the game has actually started
        if ((levelManager != null && !levelManager.gameStarted) || (endlessManager != null && !endlessManager.gameStarted)) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(isPaused);

        // Resume/Countdown logic (if not paused)
       if (!isPaused)
        {
            GameStartManager countdownScript = Object.FindFirstObjectByType<GameStartManager>(FindObjectsInactive.Include);
            if (countdownScript != null)
            {
                // Ensure the game is frozen while the countdown is visible
                Time.timeScale = 0f; 

                // Reset game state flags
                if (levelManager != null) levelManager.gameStarted = false;
                if (endlessManager != null) endlessManager.gameStarted = false;

                // Ensure the countdown panel is visible
                if (levelManager != null && levelManager.countdownPanel != null) 
                    levelManager.countdownPanel.SetActive(true);
                else if (endlessManager != null && endlessManager.countdownPanel != null) 
                    endlessManager.countdownPanel.SetActive(true);

                countdownScript.gameObject.SetActive(true);
                if (countdownScript.countdownText != null) 
                    countdownScript.countdownText.gameObject.SetActive(true);

                countdownScript.StopAllCoroutines();
                countdownScript.StartCoroutine("StartCountdown");
            }
            else
            {
                // No countdown exists, resume immediately
                Time.timeScale = 1f;
            }
        }
    }

    // --- QUIT LOGIC ---
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void Update()
    {
        // 1. Existing Pause Toggle Logic
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        // 2. Existing Popup Dismissal Logic
        if (popupActive)
        {
            bool interaction = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || 
                               (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

            if (interaction)
            {
                noMoreLevelsPanel.SetActive(false);
                popupActive = false;
                
                // Ensure time resumes if the popup closes
                Time.timeScale = 1f;
            }
        }
    }

    public void PromptRestartCampaign() 
{
        Object.FindFirstObjectByType<ReturnConfirmation>().OpenPopup("RestartCampaign");
}

    public void PromptRestartEndless() 
    {
        Object.FindFirstObjectByType<ReturnConfirmation>().OpenPopup("RestartEndless");
    }

    public void PromptQuitCampaign() 
    {
        Object.FindFirstObjectByType<ReturnConfirmation>().OpenPopup("QuitCampaign");
    }
}