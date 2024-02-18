using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float maxFlyingForce = 20f;
    private float currentFlyingForce = 0f;
    public float spiralStrength = 1f; // 螺旋强度
    private bool grounded = false;
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentFlyingForce = 0f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = Vector2.zero;
                currentFlyingForce += Time.deltaTime * maxFlyingForce;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                if (currentFlyingForce <= maxFlyingForce)
                {
                    rb.AddForce(transform.right * currentFlyingForce, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(transform.up * spiralStrength, ForceMode2D.Force);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            currentFlyingForce = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
