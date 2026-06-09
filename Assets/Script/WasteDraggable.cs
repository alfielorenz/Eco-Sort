using UnityEngine;
using UnityEngine.InputSystem;

public class WasteDraggable : MonoBehaviour
{
    private bool isDragging = false;
    private Rigidbody2D rb;
    private Collider2D col; 
    private Camera mainCamera;
    private LevelManager levelManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        mainCamera = Camera.main;
        levelManager = Object.FindAnyObjectByType<LevelManager>();
    }

    void Update()
    {
        
        if (levelManager != null && !levelManager.gameStarted) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (col == Physics2D.OverlapPoint(mousePos)) StartDragging();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging) StopDragging();
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            rb.MovePosition(mousePos);
        }
    }

    void StartDragging() { isDragging = true; rb.gravityScale = 0; col.isTrigger = true; }
    void StopDragging() { isDragging = false; rb.gravityScale = 1; col.isTrigger = false; }
}