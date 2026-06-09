using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinDisplay : MonoBehaviour
{
    public Image starDisplay;
   
    public Sprite[] starSprites; 

    void Start()
    {
        
        int stars = PlayerPrefs.GetInt("LastStarsEarned", 1);

        if (starDisplay != null && starSprites.Length >= 3)
        {
           
            int spriteIndex = Mathf.Clamp(stars - 1, 0, starSprites.Length - 1);
            starDisplay.sprite = starSprites[spriteIndex];
        }
    }

    public void NextLevel()
    {
        
        int current = GameMemory.currentLevelNumber;
        SceneManager.LoadScene("Level " + (current + 1));
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Campaign");
    }
}