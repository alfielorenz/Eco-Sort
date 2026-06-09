using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class TriviaQuestion
{
    public string questionText;
    public string[] choices = new string[3];
    public int correctChoiceIndex; 
    [TextArea]
    public string explanation;
}

public class TriviaManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject triviaPanel;
    public GameObject explanationPanel;

    [Header("Text References")]
    public TextMeshProUGUI questionTextDisplay;
    public TextMeshProUGUI explanationTextDisplay;

    [Header("Buttons")]
    public Button[] choiceButtons;
    public Button resumeButton; 

    [Header("Trivia Questions")]
    public List<TriviaQuestion> triviaList;

    private GameObject pendingDestroyItem; 
    public SupervisorController supervisor;
    void Start()
    {
        if (triviaPanel) triviaPanel.SetActive(false);
        if (explanationPanel) explanationPanel.SetActive(false);
        if (resumeButton) resumeButton.gameObject.SetActive(false);
        
        if (resumeButton)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(CloseTrivia);
        }
    }

    public void OnWrongSorting(GameObject item)
    {
        pendingDestroyItem = item; 
        TriggerTrivia();
    }

    public void TriggerTrivia()
    {
        if (triviaList == null || triviaList.Count == 0)
        {
            if (pendingDestroyItem != null) Destroy(pendingDestroyItem);
            return;
        }

        Time.timeScale = 0; 
        
        if (triviaPanel) triviaPanel.SetActive(true);
        if (explanationPanel) explanationPanel.SetActive(false);
        if (resumeButton) resumeButton.gameObject.SetActive(false);

        TriviaQuestion currentQuestion = triviaList[Random.Range(0, triviaList.Count)];
        DisplayQuestion(currentQuestion);
    }

    void DisplayQuestion(TriviaQuestion currentQuestion)
    {
        if (questionTextDisplay == null) return;
        questionTextDisplay.text = currentQuestion.questionText;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i >= currentQuestion.choices.Length) break;

            choiceButtons[i].interactable = true;
            TextMeshProUGUI btnText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = currentQuestion.choices[i];

            int index = i; 
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => HandleChoice(index, currentQuestion));
        }
    }

    void HandleChoice(int index, TriviaQuestion question)
    {
        foreach (Button b in choiceButtons) b.interactable = false;

        bool isCorrect = (index == question.correctChoiceIndex);
        
        if (!isCorrect)
        {
            // Level Mode Check
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.DeductScore(); 
            }
            // Endless Mode Fallback
            else if (ScoreManagerEndless.Instance != null)
            {
                ScoreManagerEndless.Instance.DeductScore(); 
            }
        }

        if (explanationTextDisplay != null)
        {
            explanationTextDisplay.text = isCorrect 
                ? $"<color=green>Correct!</color>\n{question.explanation}" 
                : $"<color=red>Incorrect!</color>\n{question.explanation}";
        }

        if (explanationPanel) explanationPanel.SetActive(true);
        if (resumeButton) resumeButton.gameObject.SetActive(true);
    }

    public void CloseTrivia()
    {
        if (triviaPanel) triviaPanel.SetActive(false);
        if (explanationPanel) explanationPanel.SetActive(false);
        if (resumeButton) resumeButton.gameObject.SetActive(false);

        if (pendingDestroyItem != null)
        {
            Destroy(pendingDestroyItem);
        }

        Time.timeScale = 1f; 

        if (supervisor != null)
        {
            supervisor.EndTrivia();
        }
    }
}