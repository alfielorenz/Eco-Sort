using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject countdownPanel;

    [HideInInspector] public bool gameStarted = false; 
    [HideInInspector] public bool isGameOver = false;

    private void Start()
    {
        gameStarted = false;
        isGameOver = false;

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        
        GameMemory.lastPlayedLevel = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
       
        if (gameStarted && ScoreManagerEndless.Instance != null)
        {
            GameMemory.pointsEarned = ScoreManagerEndless.Instance.currentScore;
        }
    }

    public void StartLevel()
    {
       
        GameObject triviaPanel = GameObject.Find("TriviaPanelName");
        if (triviaPanel != null && triviaPanel.activeSelf)
        {
            Time.timeScale = 0f; 
            return; 
        }
        // -------------------

        gameStarted = true;
        Time.timeScale = 1f; 

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }
    }
}