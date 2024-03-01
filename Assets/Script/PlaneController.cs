using UnityEngine;
using UnityEngine.Serialization;

public class PlaneController : MonoBehaviour
{
    public float spiralSpeed;
    public float minSpiralRadius;
    public float maxSpiralRadius;
    public float radiusSlope = 0.1f;

    //public float spiralForceThreshold;
    public float holdingTimeThreshold = 0.5f;
    private float _currentRadius;
    [HideInInspector] public float holdingTimeCnter;
    private float _previousAngle;

    private float _spiralStrength;
    private float currentFlyingForce;
    private bool grounded;
    private bool isSpiraling;
    private Vector2 lastVelocity;
    public float maxFlyingForce = 20f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _currentRadius = maxSpiralRadius;
    }

    private void Update()
    {
        if(spee
        if (!grounded)
        {   
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
            {
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
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                if (holdingTimeCnter < holdingTimeThreshold * 0.5f)
                {
                    currentFlyingForce = holdingTimeCnter * maxFlyingForce * 0.3f;//
                    rb.AddForce(transform.up * currentFlyingForce, ForceMode2D.Impulse);
                }
                else
                {
                    currentFlyingForce = holdingTimeCnter * maxFlyingForce;
                    _currentRadius += radiusSlope * holdingTimeCnter; // (_holdingTimeCnter - 0.5f)
                    Debug.Log(_currentRadius);
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
