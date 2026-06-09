using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshPro

public class LevelSelectionManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelPopupData
    {
        public string sceneName;      
        public GameObject levelPanel; 
        
        [Header("Display UI")]
        public TextMeshProUGUI highScoreText; 
        public Image starStatusDisplay;       
        
        [Header("Star Sprites (4 States)")]
        public Sprite zeroStarsSprite;  
        public Sprite oneStarSprite;   
        public Sprite twoStarsSprite;  
        public Sprite threeStarsSprite; 
    }

    public LevelPopupData[] levelPopups;
    private GameObject currentActivePanel;

    void Awake()
    {
        ForceHideAll();
    }

    private void ForceHideAll()
    {
        if (levelPopups != null)
        {
            foreach (var data in levelPopups)
            {
                if (data.levelPanel != null) data.levelPanel.SetActive(false);
            }
        }
    }

    public void OpenSpecificPopup(int index)
    {
        CloseCurrentPopup();

        if (index >= 0 && index < levelPopups.Length)
        {
            LevelPopupData data = levelPopups[index];
            currentActivePanel = data.levelPanel;

            if (currentActivePanel != null)
            {
                
                int highScore = PlayerPrefs.GetInt(data.sceneName + "_HighScore", 0);
                int starCount = PlayerPrefs.GetInt(data.sceneName + "_Stars", 0);

                
                if (data.highScoreText != null) 
                    data.highScoreText.text = highScore.ToString();

                if (data.starStatusDisplay != null)
                {
                    if (starCount == 3) data.starStatusDisplay.sprite = data.threeStarsSprite;
                    else if (starCount == 2) data.starStatusDisplay.sprite = data.twoStarsSprite;
                    else if (starCount == 1) data.starStatusDisplay.sprite = data.oneStarSprite;
                    else data.starStatusDisplay.sprite = data.zeroStarsSprite;
                }

                currentActivePanel.SetActive(true);
                currentActivePanel.transform.SetAsLastSibling(); 
            }
        }
    }

    public void CloseCurrentPopup()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
        }
    }

    public void PlayLevel(string sceneName)
{
        // 1. Extract the number from the scene name (e.g., "Level 6" -> 6)
        string numberPart = System.Text.RegularExpressions.Regex.Match(sceneName, @"\d+").Value;
        
        if (int.TryParse(numberPart, out int levelNum))
        {
            GameMemory.currentLevelNumber = levelNum;
            Debug.Log("Campaign starting at level: " + GameMemory.currentLevelNumber);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
}
}