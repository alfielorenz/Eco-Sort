using UnityEngine;

public class WorldButton : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

  
    private void OnMouseDown()
    {
        if (anim != null)
        {
            anim.SetTrigger("Pressed");
        }

        Debug.Log("Button Clicked!");
    }

    
    private void OnMouseUp()
    {
        if (anim != null)
        {
            anim.SetTrigger("Normal");
        }
    }
}