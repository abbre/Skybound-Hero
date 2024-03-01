using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;


public class test : MonoBehaviour
{
    private bool isDPressed = false;
    private bool isWPressed = false;
    private bool keyPressed = false;
    private bool canSpace = false;
    public float spiralSpeed;
    public float minSpiralRadius;
    public float maxSpiralRadius;
    public float radiusSlope = 0.1f;
    public float holdingTimeThreshold = 0.5f;
    private float _currentRadius;
    [HideInInspector]public float holdingTimeCnter;
    private float _previousAngle;
    private float _spiralStrength;
    private float currentFlyingForce;
    private bool grounded;
    private bool isSpiraling;
    private Vector2 lastVelocity;
    public float maxFlyingForce = 20f;
    private Quaternion initialRotation;
    public float stopTime = 1.0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        //1.按下wd飞机停止，风向animation，防止两个键同时按
        if (!grounded)
        {
            if (Input.GetKey(KeyCode.D))
            {
                isDPressed = true;
                keyPressed = true;
                isWPressed = false;
            }
            if (Input.GetKey(KeyCode.W))
            {
                isWPressed = true;
                keyPressed = true;
                isDPressed = false;
            }
            //风向的判断

            if (keyPressed)
            {   
                StartCoroutine(StopAndRotate());
            }
            
            //2.按下空格键，计时，蓄力条
            if (canSpace)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    holdingTimeCnter += Time.deltaTime;
                }
            
            //3.松开空格键：如果按w，飞机向上转圈，时间越长转的越快；如果按d，飞机滑行
                if (Input.GetKeyUp(KeyCode.Space) && isDPressed)
                 {
                    if (holdingTimeCnter < holdingTimeThreshold)
                    {
                        currentFlyingForce = holdingTimeCnter * maxFlyingForce * 1f;
                        rb.AddForce(transform.right * currentFlyingForce, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(transform.right * maxFlyingForce, ForceMode2D.Impulse);
                    }
                }

                if (Input.GetKeyUp(KeyCode.Space) && isWPressed)
                {
                    if (holdingTimeCnter < 0.5f * holdingTimeThreshold)
                    {
                        rb.AddForce(transform.up * currentFlyingForce * 0.8f, ForceMode2D.Impulse);
                    }
                    else
                    {
                        currentFlyingForce = holdingTimeCnter * maxFlyingForce;
                        _currentRadius += radiusSlope * holdingTimeCnter;

                        _spiralStrength = currentFlyingForce; 
                        isSpiraling = true;

                        if (_currentRadius < minSpiralRadius)
                        {
                            _currentRadius = minSpiralRadius;

                        }
                        else if (_currentRadius > maxSpiralRadius)
                        {
                            _currentRadius = maxSpiralRadius;
                        }
                        
                        spiralSpeed = Mathf.Sqrt(currentFlyingForce * _currentRadius);
                        rb.velocity = new Vector2(spiralSpeed, 0f);

                    }
                    _currentRadius = minSpiralRadius;
                    currentFlyingForce = 0f;
                    holdingTimeCnter = 0;
                }

                if (isSpiraling)
                {
                    rb.gravityScale = 0f;
                    var perpendicular = new Vector2(-lastVelocity.y, lastVelocity.x).normalized; // 获得与速度方向垂直的向心力方向
                    rb.AddForce(perpendicular * _spiralStrength, ForceMode2D.Force);

                    lastVelocity = rb.velocity;

                    // The angle of the plane internal rotation
                    var angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    if (_previousAngle < 0 && angle >= 0) // detect if the plane has rotated one circle 
                    {
                        isSpiraling = false;
                        angle = 0;
                        rb.gravityScale = 1f; // set the gravity back to make the plane drop
                    }

                    _previousAngle = angle;
                }
            }
        }
    }

    IEnumerator StopAndRotate()
    {
        float elapsedTime = 0f;
        Vector2 initialVelocity = rb.velocity;

        while (elapsedTime < stopTime)
        {
            float t1 = elapsedTime / stopTime;
            rb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t1);
            elapsedTime += Time.deltaTime;

            float t2 = Mathf.PingPong(Time.time * spiralSpeed, 1.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, t2);

            yield return null;
        }

        rb.velocity = Vector2.zero;
        canSpace = true;
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
}