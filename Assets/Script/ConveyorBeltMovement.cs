using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("How fast the texture moves")]
    public float scrollSpeed = 0.5f;
    [Tooltip("Direction of the scroll (x: 1 for right, -1 for left)")]
    public Vector2 scrollDirection = Vector2.right;

    [Header("Physics Settings")]
    [Tooltip("How fast objects are pushed")]
    public float beltSpeed = 2.0f;

    private Renderer rend;
    private Rigidbody2D rb2d;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb2d = GetComponent<Rigidbody2D>();

        
        if (rend != null && rend.material.mainTexture != null)
        {
            rend.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }
    }

    void Update()
    {
        
        float offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = scrollDirection * offset;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D targetRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (targetRb != null)
        {
            Vector2 velocity = scrollDirection * beltSpeed;
            targetRb.linearVelocity = new Vector2(velocity.x, targetRb.linearVelocity.y);
        }
    }
}