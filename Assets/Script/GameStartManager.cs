using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartManager : MonoBehaviour
{
    public LevelManager levelManager;
    public EndlessManager endlessManager; 
    public TextMeshProUGUI countdownText;
    public float countdownDuration = 3f;

    private void Start()
    {
        
        if (levelManager == null)
            levelManager = Object.FindFirstObjectByType<LevelManager>();

        if (endlessManager == null)
            endlessManager = Object.FindFirstObjectByType<EndlessManager>();
            
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        float timer = countdownDuration;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            yield return new WaitForSecondsRealtime(1f); 
            timer--;
        }

        countdownText.text = "START!";
        
        // 1. Trigger the logic to unfreeze time
        if (levelManager != null)
        {
            levelManager.StartLevel(); 
        }
        else if (endlessManager != null)
        {
            endlessManager.StartLevel(); 
        }

        // 2. Force a small delay to let the game stabilize before UI cleans up
        yield return new WaitForSecondsRealtime(1f);
        
        // 3. Ensure this is the absolute last thing that happens
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(1f);
        countdownText.gameObject.SetActive(false);
    }
}