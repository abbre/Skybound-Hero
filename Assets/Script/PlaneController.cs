using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float spiralSpeed;
    public float radiusDiameter; //此数值和实半径大小成反比，若要调整飞行半径，只需要调整这个数值

    public float holdingTimeThreshold = 0.5f; //按下多久
    [HideInInspector] public float holdingTimeCnter;
    private float _previousAngle;

    private float _previousHeight;
    private float _spiralStrength;
    private float currentFlyingForce;
    private bool grounded;
    private bool isSpiraling;
    private Vector2 lastVelocity;

    //对于
    public float maxFlyingForceHorizontal;
    public float maxFlyingForceSpiral;

    private bool _motionChosen = false;
    private bool _spiralMotionChosen = false;
    private bool _horizontalMotionChosen = false;
    private bool _speedZero = false;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // _currentRadius = maxSpiralRadius;
    }

    private void Update()
    {
        if (!grounded)
        {
            if (!_motionChosen)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _speedZero = true;
                    rb.velocity = Vector2.zero;
                    _spiralMotionChosen = true;
                    _motionChosen = true;
                    Debug.Log("spiralchosen");
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _speedZero = true;
                    _horizontalMotionChosen = true;
                    _motionChosen = true;
                    Debug.Log("horizontalchosen");
                }
            }

            if (_motionChosen) //如果玩家按下方向选择键
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    rb.velocity = Vector2.zero;
                    holdingTimeCnter += Time.deltaTime;
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (_horizontalMotionChosen) //选择平抛模式
                    {
                        currentFlyingForce = holdingTimeCnter * maxFlyingForceHorizontal;
                        rb.AddForce(transform.right * currentFlyingForce, ForceMode2D.Impulse);
                        _motionChosen = false;
                        _horizontalMotionChosen = false;
                    } else if (_spiralMotionChosen) //选择旋转模式
                    {
                        currentFlyingForce = holdingTimeCnter * maxFlyingForceSpiral * radiusDiameter;

                        _spiralStrength = currentFlyingForce;
                        isSpiraling = true;

                        rb.velocity = new Vector2(spiralSpeed, 0f);
                    }
                    currentFlyingForce = 0f;
                    
                   
                    
                }
               
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
                    _spiralMotionChosen = false;
                    _motionChosen = false;
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