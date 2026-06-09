using UnityEngine;
using TMPro;

public class EndlessModeStats : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject statsPopupPanel; 

    [Header("UI Assignments")]
    [SerializeField] private TextMeshProUGUI currentScoreDisplay;
    [SerializeField] private TextMeshProUGUI currentTimeDisplay;
    [SerializeField] private TextMeshProUGUI bestScoreDisplay;
    [SerializeField] private TextMeshProUGUI bestTimeDisplay;

    // Call this to show the popup
    public void OpenStatsPopup()
    {
        if (statsPopupPanel != null)
        {
            UpdateStatsDisplay(); // Refresh data before opening
            statsPopupPanel.SetActive(true);
            statsPopupPanel.transform.SetAsLastSibling();
        }
    }

    // Call this to hide the popup
    public void CloseStatsPopup()
    {
        if (statsPopupPanel != null) statsPopupPanel.SetActive(false);
    }

    private void UpdateStatsDisplay()
    {
        int currentScore = PlayerPrefs.GetInt("EndlessSavedPoints", 0);
        float currentTime = PlayerPrefs.GetFloat("EndlessSavedTime", 0f);
        int bestScore = PlayerPrefs.GetInt("EndlessHighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("EndlessBestTime", 0f);

        if (currentScoreDisplay != null) currentScoreDisplay.text = currentScore.ToString();
        if (bestScoreDisplay != null) bestScoreDisplay.text = bestScore.ToString();
        if (currentTimeDisplay != null) currentTimeDisplay.text = FormatTime(currentTime);
        if (bestTimeDisplay != null) bestTimeDisplay.text = FormatTime(bestTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}