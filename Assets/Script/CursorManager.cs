using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Skins")]
    public Texture2D openGlove;
    public Texture2D closedGlove;

    [Header("Settings")]
    public Vector2 hotSpot = new Vector2(32, 32); 

    void Start()
    {
        SetOpenHand();
    }

    void Update()
    {
        // Only trigger when the button is first clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SetClosedHand();
        }
        // Only trigger when the button is released
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SetOpenHand();
        }
    }

    public void SetOpenHand()
    {
        Cursor.SetCursor(openGlove, hotSpot, CursorMode.Auto);
    }

    public void SetClosedHand()
    {
        Cursor.SetCursor(closedGlove, hotSpot, CursorMode.Auto);
    }
}