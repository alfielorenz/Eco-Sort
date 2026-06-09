using UnityEngine;
using System.Collections;

public class SupervisorController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine; // Store the routine reference

    [Header("Sprites")]
    public Sprite watchingSprite;
    public Sprite happySprite;
    public Sprite angrySprite;
    public Sprite blinkSprite;

    private bool isBusy = false;

    void Start()
{
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        ApplyEquippedSkin(); 
        
        if (watchingSprite != null) spriteRenderer.sprite = watchingSprite;
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    void ApplyEquippedSkin()
{
        string skinID = PlayerPrefs.GetString("EquippedSkin", "Default");
        // Find the skin data in the manager
        SkinManager.SkinData data = System.Array.Find(SkinManager.Instance.allSkins, s => s.skinID == skinID);
        
        if (data != null && skinID != "Default")
        {
            watchingSprite = data.watchingSprite;
            happySprite = data.happySprite;
            angrySprite = data.angrySprite;
            blinkSprite = data.blinkSprite;
            
            spriteRenderer.sprite = watchingSprite;
        }
}
    public void PlayHappy()
    {
        if (isBusy) return;
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        StartCoroutine(TemporaryExpression(happySprite, 1.0f));
    }

    public void PlayAngry()
    {
        isBusy = true;
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        spriteRenderer.sprite = angrySprite;
    }

    public void EndTrivia()
    {
        Debug.Log("EndTrivia called! Resetting sprite.");
        StopAllCoroutines(); 
        isBusy = false;
        spriteRenderer.sprite = watchingSprite; 
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    IEnumerator TemporaryExpression(Sprite expression, float duration)
    {
        isBusy = true;
        spriteRenderer.sprite = expression;
        yield return new WaitForSecondsRealtime(duration); 
        
        isBusy = false;
        spriteRenderer.sprite = watchingSprite;
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(4f, 8f));
            
            if (!isBusy)
            {
                spriteRenderer.sprite = blinkSprite;
                yield return new WaitForSecondsRealtime(0.3f);
                spriteRenderer.sprite = watchingSprite;
            }
        }
    }
}