using UnityEngine;
using TMPro;
using System.Collections;

public class LevelStartCountdown : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;

    [Header("Settings")]
    [SerializeField] private int countdownTime = 3;

    void Start()
    {
        
        StartCoroutine(RunCountdown());
    }

    IEnumerator RunCountdown()
    {
        if (countdownPanel != null) countdownPanel.SetActive(true);
        
        int currentCount = countdownTime;

        while (currentCount > 0)
        {
            if (countdownText != null) countdownText.text = currentCount.ToString();
            
            yield return new WaitForSecondsRealtime(1f);
            currentCount--;
        }

        if (countdownText != null) countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

       
        LevelManager lm = Object.FindAnyObjectByType<LevelManager>();
        if (lm != null)
        {
            lm.StartLevel();
        }
        else
        {
            
            if (countdownPanel != null) countdownPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}