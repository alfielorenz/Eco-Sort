using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

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
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
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