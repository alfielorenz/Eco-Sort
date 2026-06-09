using UnityEngine;

public class EndlessConveyor : MonoBehaviour
{
    [Header("Conveyor Settings")]
    public float speed = 2.0f;
    public Vector2 direction = Vector2.right;

    [Header("Physics Settings")]
    public ContactFilter2D contactFilter;
    private Rigidbody2D rb;
    private BoxCollider2D beltCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        beltCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        Collider2D[] results = new Collider2D[15]; 
        int count = rb.Overlap(contactFilter, results);

        for (int i = 0; i < count; i++)
        {
            Rigidbody2D itemRb = results[i].GetComponent<Rigidbody2D>();

            if (itemRb != null)
            {
                
                if (IsOverBelt(results[i]))
                {
                    Vector2 movement = direction.normalized * speed * Time.fixedDeltaTime;
                    itemRb.position += movement;

                   
                    itemRb.linearVelocity = new Vector2(direction.x * speed, itemRb.linearVelocity.y);
                }
                
            }
        }
    }

    bool IsOverBelt(Collider2D other)
    {
        
        float edge = direction.x > 0 ? beltCollider.bounds.max.x : beltCollider.bounds.min.x;

        if (direction.x > 0 && other.bounds.center.x > edge) return false;
        if (direction.x < 0 && other.bounds.center.x < edge) return false;

        return true;
    }
}