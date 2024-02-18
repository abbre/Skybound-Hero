using UnityEngine;

public class SpiralMotion : MonoBehaviour
{
    public float spiralStrength = 1f; // 螺旋强度

    private Rigidbody2D rb;
    private bool isSpiraling = false;
    private Vector2 lastVelocity;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastVelocity = rb.velocity;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpiraling = true;
        }

        if (isSpiraling)
        {
            Vector2 perpendicular = new Vector2(-lastVelocity.y, lastVelocity.x).normalized;
            
            rb.AddForce(perpendicular * spiralStrength, ForceMode2D.Force);
            
            lastVelocity = rb.velocity;
        }
    }
}