using UnityEngine;
using TMPro;

public class ScoreManagerEndless : MonoBehaviour
{
    public static ScoreManagerEndless Instance;

    [Header("Score Settings")]
    public int currentScore = 0;
    public TextMeshProUGUI scoreText;

    [Header("GDD Values")]
    public int pointsPerWaste = 10; 
    public int triviaPenalty = 50;  

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load saved score from PlayerPrefs; default to 0 if not found
        currentScore = PlayerPrefs.GetInt("EndlessSavedPoints", 0);
        UpdateScoreUI();
    }
    
    public void AddScore()
    {
        currentScore += pointsPerWaste;
        UpdateScoreUI();
    }

    public void DeductScore()
    {
        currentScore -= triviaPenalty;
        if (currentScore < 0) currentScore = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }
}