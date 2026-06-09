using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverQuit : MonoBehaviour
{
    [SerializeField] private string campaignMenuScene = "Campaign";
    [SerializeField] private string endlessMenuScene = "LevelMode";

    public void OnQuitButtonClick()
    {
        if (GameMemory.isEndlessMode)
        {
            SceneManager.LoadScene(endlessMenuScene);
        }
        else
        {
            SceneManager.LoadScene(campaignMenuScene);
        }
    }
}