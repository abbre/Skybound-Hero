using UnityEngine;
using System.Collections;
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
    [HideInInspector] public bool isSpiraling;
    private bool isRotating = false;
    
    //飞机自身旋转角度
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
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                if (!isRotating)
                {
                    StartCoroutine(RotateToInitialRotation());
                }
            }

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

                //transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
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
                //在最小和最大半径范围内返回半径数值
                float holdingTimeRatio = Mathf.InverseLerp(0, holdingTimeThreshold, holdingTimeCnter);
                float currentRadius = Mathf.Lerp(minSpiralRadius, maxSpiralRadius, holdingTimeRatio);

                //当前坐标向上半径大小，获取圆心位置
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

                //每一帧的旋转角度
                float deltaRotation = spiralSpeed * Time.deltaTime;
                angleRotated += deltaRotation;

                transform.RotateAround(spiralCenter, Vector3.forward, deltaRotation);

                //前半圈速度小，且大于最小速度
                if (transform.position.y > previousPosition.y)
                {
                    spiralSpeed = Mathf.Max(spiralSpeed - Time.deltaTime * 80f, minSpiralSpeed); 
                }
                else
                {
                    //后半圈速度大，且小于最大速度
                    spiralSpeed = Mathf.Min(spiralSpeed + Time.deltaTime * 80f, maxSpiralSpeed); 
                }
                
                previousPosition = transform.position; 

                
                //转完一圈
                if (angleRotated >= 360f)
                {
                    rb.gravityScale = 1f; 
                    isSpiraling = false;
                }
            }
        }
    }

    private IEnumerator RotateToInitialRotation()
{
    isRotating = true;
    float t = 0f;
    while (t < 0.5f)
    {
        t += Time.deltaTime * 1f;
        transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, t);
        yield return null;
    }
    isRotating = false;
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
