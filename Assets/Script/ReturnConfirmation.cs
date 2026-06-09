using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReturnConfirmation : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject areYouSurePanel; 

    [Header("Pause Menu Buttons")]
    [SerializeField] private List<Button> pauseMenuButtons;

    // Define the actions
    private enum ActionType { None, RestartCampaign, RestartEndless, QuitCampaign }
    private ActionType currentAction;

    private void Start()
    {
        if (areYouSurePanel != null) areYouSurePanel.SetActive(false);
    }

    public void OpenPopup(string action)
    {
        Debug.Log("OpenPopup called for: " + action); 
        
        currentAction = (ActionType)System.Enum.Parse(typeof(ActionType), action);
        
        if (areYouSurePanel != null) 
        {
            areYouSurePanel.SetActive(true);
            Debug.Log("Panel should now be active.");
        }
        else 
        {
            Debug.LogError("AreYouSurePanel is not assigned in the Inspector!");
        }
        
        SetButtonsInteractable(false);
}

    public void OnClickYes()
    {
        switch (currentAction)
        {
            case ActionType.RestartCampaign:
                PlayerPrefs.DeleteKey("CampaignSavedPoints");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case ActionType.RestartEndless:
                PlayerPrefs.DeleteKey("EndlessSavedPoints");
                PlayerPrefs.DeleteKey("EndlessSavedTime");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case ActionType.QuitCampaign:
                SceneManager.LoadScene("Campaign"); 
                break;
        }
        Time.timeScale = 1f;
    }

    public void ClosePopup()
    {
        if (areYouSurePanel != null) areYouSurePanel.SetActive(false);
        SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        foreach (Button button in pauseMenuButtons)
        {
            if (button != null) button.interactable = interactable;
        }
    }
}