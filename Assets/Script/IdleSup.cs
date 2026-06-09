using UnityEngine;
using System.Collections;

public class BlinkController : MonoBehaviour
{
    private Animator animator;

    
    public float minBlinkDelay = 4f;
    public float maxBlinkDelay = 8f;

    
    public float blinkSpeed = 1f;

    
    public float blinkDuration = 0.15f;

    
    public float doubleBlinkChance = 0.3f;

    void Start()
    {
        animator = GetComponent<Animator>();

       
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0.99f);
        animator.speed = 0f;

        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
           
            float waitTime = Random.Range(minBlinkDelay, maxBlinkDelay);
            yield return new WaitForSeconds(waitTime);

           
            yield return StartCoroutine(DoBlink());

           
            if (Random.value < doubleBlinkChance)
            {
                yield return new WaitForSeconds(0.2f); 
                yield return StartCoroutine(DoBlink());
            }
        }
    }

    IEnumerator DoBlink()
    {
       
        animator.speed = blinkSpeed;
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0f);

        
        yield return new WaitForSeconds(blinkDuration);

        
        animator.speed = 0f;
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0.99f);
    }
}