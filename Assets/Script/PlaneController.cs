using UnityEngine;
using UnityEngine.Serialization;

public class PlaneController : MonoBehaviour
{
    public float spiralSpeed = 100f; // 控制旋转速度
    public float maxFlyingForce = 20f;
    public float minSpiralRadius = 5f;
    public float maxSpiralRadius = 50f;
    public float holdingTimeThreshold = 1f;
    public float maxSpiralSpeed;
    public float minSpiralSpeed;

    public GameObject windUp;
    public GameObject windRight;

    [HideInInspector] public float holdingTimeCnter;
    private Vector2 spiralCenter;
    private bool isSpiraling;
    private float angleRotated = 0f;

    private Vector2 previousPosition;

    private float currentFlyingForce;
    private bool grounded;

    private Quaternion initialRotation;
    private Vector2 initialVelocity;
    private float initialAngularVelocity;
 
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousPosition = transform.position;

        initialRotation = transform.rotation;
        initialVelocity = rb.velocity;
        initialAngularVelocity = rb.angularVelocity;

        windUp.SetActive(false);
        windRight.SetActive(false);
    }

    private void Update()
    {
        if (!grounded)
        {   
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.D))
                {
                    windRight.SetActive(true);
                    windUp.SetActive(false);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    windUp.SetActive(true);
                    windRight.SetActive(false);
                }

                isSpiraling = false;
                rb.gravityScale = 1f; 
                rb.velocity = Vector2.zero;
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                holdingTimeCnter += Time.deltaTime;
            }
            
            if (Input.GetKeyUp(KeyCode.D))
            {
                if (holdingTimeCnter < holdingTimeThreshold)
                {
                    currentFlyingForce = holdingTimeCnter * maxFlyingForce;
                    rb.AddForce(transform.right * currentFlyingForce, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(transform.right * maxFlyingForce, ForceMode2D.Impulse);
                }

                holdingTimeCnter = 0;
                windRight.SetActive(false);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                float holdingTimeRatio = Mathf.InverseLerp(0, holdingTimeThreshold, holdingTimeCnter);
                float currentRadius = Mathf.Lerp(minSpiralRadius, maxSpiralRadius, holdingTimeRatio);

                spiralCenter = (Vector2)transform.position + new Vector2(0, currentRadius);
                    
                isSpiraling = true;
                angleRotated = 0f; 
                holdingTimeCnter = 0;
                windUp.SetActive(false);
            }

            if (isSpiraling)
            {
                rb.gravityScale = 0;
                float step = spiralSpeed * Time.deltaTime;

                
                float rotationThisFrame = spiralSpeed * Time.deltaTime;
                angleRotated += rotationThisFrame;

                transform.RotateAround(spiralCenter, Vector3.forward, rotationThisFrame);

                if (transform.position.y > previousPosition.y)
                {
                    spiralSpeed = Mathf.Max(spiralSpeed - Time.deltaTime * 80f, minSpiralSpeed); 
                }
                else
                {
                    spiralSpeed = Mathf.Min(spiralSpeed + Time.deltaTime * 80f, maxSpiralSpeed); 
                }

                previousPosition = transform.position; 

                if (angleRotated >= 360f)
                {
                    rb.gravityScale = 1f; 
                    isSpiraling = false;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            currentFlyingForce = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bound") || other.CompareTag("RedArea"))
        {   
            rb.velocity = initialVelocity;
            rb.angularVelocity = initialAngularVelocity;
            transform.rotation = initialRotation;

            rb.gravityScale = 1f; 
            isSpiraling = false;
            windUp.SetActive(false);
            windRight.SetActive(false);
        }
    }
}
