using UnityEngine;
using TMPro; 

public class TimerEndless : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    void Start()
    {
        // Load saved time from PlayerPrefs; default to 0 if not found
        elapsedTime = PlayerPrefs.GetFloat("EndlessSavedTime", 0f);
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}