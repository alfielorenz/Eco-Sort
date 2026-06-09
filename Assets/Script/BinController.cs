using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BinController : MonoBehaviour
{
    private Animator anim;
    
    [Header("Bin Config")]
    public string correctTag; 

    [Header("References")]
    public TriviaManager triviaManager;
    
    public SupervisorController supervisor; 

    private List<GameObject> hoveredItems = new List<GameObject>();

    void Start() { anim = GetComponent<Animator>(); }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!hoveredItems.Contains(other.gameObject)) hoveredItems.Add(other.gameObject);
        UpdateAnimation();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        hoveredItems.Remove(other.gameObject);
        UpdateAnimation();
    }

    void Update() 
    {
        if (Time.timeScale == 0) return; 
        if (Mouse.current.leftButton.wasReleasedThisFrame && hoveredItems.Count > 0)
        {
            AcceptItem(hoveredItems[hoveredItems.Count - 1]);
        }
    }

    void UpdateAnimation() { if (anim != null) anim.SetBool("isOpen", hoveredItems.Count > 0); }

    void AcceptItem(GameObject item) 
    {
        hoveredItems.Remove(item);
        UpdateAnimation();

        if (item.CompareTag(correctTag)) 
        {
          
            if (supervisor != null) supervisor.PlayHappy();
            
            if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore();
            else if (ScoreManagerEndless.Instance != null) ScoreManagerEndless.Instance.AddScore();
            
            Destroy(item);
        } 
        else 
        {
            
            if (supervisor != null) supervisor.PlayAngry();
            
            if (triviaManager != null) triviaManager.OnWrongSorting(item);
            else Destroy(item);
        }

        WasteSpawner spawner = Object.FindFirstObjectByType<WasteSpawner>();
        if (spawner != null) spawner.RegisterSortedItem();
    }
}