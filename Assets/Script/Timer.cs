using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI timerText;

    private LevelManager levelManager;

    void Start()
    {
       
        levelManager = Object.FindAnyObjectByType<LevelManager>();
        
        if (levelManager == null)
        {
            Debug.LogError("Timer could not find LevelManager in the scene!");
        }
    }

    void Update()
    {
        if (levelManager != null)
        {
            
            UpdateTimerDisplay(levelManager.timeRemaining);
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
       
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}