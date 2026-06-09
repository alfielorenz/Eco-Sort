using UnityEngine;

public class LevelConveyor : MonoBehaviour
{
    // These are your class-level variables
    public float scrollSpeed = 0.5f; 
    public float beltSpeed = 1.5f;
    public float maxSpeed = 3.0f;
    public Vector2 scrollDirection = Vector2.right;

    private Material beltMaterial;
    private static readonly int MainTexOffset = Shader.PropertyToID("_MainTex");

    void Start()
    {
        int currentLevel = GameMemory.currentLevelNumber > 0 ? GameMemory.currentLevelNumber : 1;
        
        float speedIncrease = (currentLevel - 1) * 0.15f;
        
        // Speed up belt to match the spawner momentum
        scrollSpeed = Mathf.Min(0.2f + speedIncrease, maxSpeed);
        beltSpeed = Mathf.Min(1.5f + speedIncrease, maxSpeed);
        
        Debug.Log("Conveyor using beltSpeed: " + beltSpeed);

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            beltMaterial = rend.material; 
        }
    }

    void Update()
    {
        if (beltMaterial != null)
        {
            float offset = Time.time * scrollSpeed;
            beltMaterial.SetTextureOffset(MainTexOffset, scrollDirection * offset);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D targetRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (targetRb != null)
        {
            Vector2 movement = scrollDirection * beltSpeed;
            targetRb.linearVelocity = new Vector2(movement.x, targetRb.linearVelocity.y);
        }
    }
}