using UnityEngine;

public class EasyPlaneController : MonoBehaviour
{
    public float maxFlyingForce = 20f;
    private float currentFlyingForce = 0f;
    private bool grounded = false;
    private bool inWater = false;
    public GameObject childObject; // 子物体的引用
    public Sprite newSprite; // 要更换的 Sprite

    public GameObject wave;
    private WaveMovement waveMovement;

    private Rigidbody2D rb;

    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        waveMovement = wave.GetComponent<WaveMovement>(); // 获取 Wave 的移动脚本
        waveMovement.enabled = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            {
                currentFlyingForce = 0f;
            }

        if (!grounded && !inWater)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                currentFlyingForce += Time.deltaTime * maxFlyingForce;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                if (currentFlyingForce <= maxFlyingForce)
                {
                    rb.AddForce(Vector2.right * currentFlyingForce, ForceMode2D.Impulse);
                }
                else 
                {
                    rb.AddForce(Vector2.right * maxFlyingForce, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * (currentFlyingForce - maxFlyingForce) * 5, ForceMode2D.Force);
                }
            }
        }

        else if (inWater)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                currentFlyingForce += Time.deltaTime * maxFlyingForce;
                currentFlyingForce = Mathf.Clamp(currentFlyingForce, 0f, maxFlyingForce);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.AddForce(Vector2.right * currentFlyingForce, ForceMode2D.Impulse);
            }
        }

         if (rb.velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeChildSprite();
        }
    }

    void ChangeChildSprite()
    {
        if (childObject != null)
        {
            SpriteRenderer childRenderer = childObject.GetComponent<SpriteRenderer>();
            if (childRenderer != null && newSprite != null)
            {
                childRenderer.sprite = newSprite;
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
        else if (collision.gameObject.CompareTag("Water"))
        {
            inWater = true;
            // 启动Wave对象的移动
            waveMovement.enabled = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            inWater = false;
        }
    }
}
