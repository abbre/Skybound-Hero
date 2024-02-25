using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isGrounded;
    private bool isMoving;
    private bool isFlying; 
    private bool isHurt; 
    private bool inWater; 
    private bool canControl; // 新增控制方式的开关
    private bool hasAppliedForce; // 检查是否已经施加过力
    public GameObject childObject; // 子物体的引用
    public Sprite plane; 
    public Sprite boat;
    public bool helpText;

    private Rigidbody2D rb;
    public Animator anim;
    private WaveMovement waveMovement;
    public GameObject wave;

    public EasyPlaneController easyPlaneController; // 引用另一个脚本

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        waveMovement = wave.GetComponent<WaveMovement>(); 
        canControl = true;
        helpText = false;
    }

    void Update()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (canControl)
        {   
            StartChildAnimation();

            isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Ground"));

            // 检查角色是否在地面上，不在地面上且速度超过阈值时触发 "hurt" 动画
            if (!isGrounded && Mathf.Abs(rb.velocity.y) > 2f)
            {
                isHurt = true;
                anim.SetBool("isHurt", true);
            }
            else
            {
                isHurt = false;
                anim.SetBool("isHurt", false);
            }
            float horizontalInput = Input.GetAxis("Horizontal");
            if (!inWater && canControl)
            {
                rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            }

            if (horizontalInput != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            anim.SetBool("isMoving", isMoving);

            // 根据水平输入的方向翻转角色的朝向
            if (isMoving)
            {
                transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
            
            if (inWater)
            {
                anim.SetBool("inWater", true);
                anim.SetBool("isMoving", false);
                anim.SetBool("isHurt", false);
            }
            else
            {
                anim.SetBool("inWater", false);
            }

            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("isFlying", true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isFlying = false;
                anim.SetBool("isFlying", false);
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {   
                StopChildAnimation();
                ChangeChildSprite();
                // 获取所有与角色发生了触发器碰撞的物体
                
                anim.SetBool("isMoving", false);
                anim.SetBool("isHurt", false);
                canControl = false; // 切换控制方式
                
                hasAppliedForce = true;
                canControl = false;
                this.enabled = false;
                easyPlaneController.enabled = true;
                easyPlaneController.Start();
                if (inWater)
                {
                    waveMovement.enabled = true;
                }
            }
        }
    }

    void StartChildAnimation()
    {
        if (childObject != null)
        {
            Animator childAnimator = childObject.GetComponent<Animator>();
            if (childAnimator != null)
            {
                childAnimator.enabled = true; 
            }
        }
    }

    void StopChildAnimation()
    {
        if (childObject != null)
        {
            Animator childAnimator = childObject.GetComponent<Animator>();
            if (childAnimator != null)
            {
                childAnimator.enabled = false; // 禁用动画组件以停止播放动画
            }
        }
    }
    void ChangeChildSprite()
    {
        if (childObject != null)
        {
            SpriteRenderer childRenderer = childObject.GetComponent<SpriteRenderer>();
            if (childRenderer != null && plane != null && boat != null)
            {
                childRenderer.sprite = inWater ? boat : plane;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.CompareTag("Ground"))
        {
            helpText = true;
            Debug.Log("HelpText is set to true");
        }

        if (other.CompareTag("Water"))
        {
            inWater = true;
            waveMovement.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.CompareTag("Ground"))
        {
            helpText = false;
            Debug.Log("HelpText is set to false");
        }

        if (other.CompareTag("Water"))
        {
            inWater = false;
            waveMovement.enabled = false;
        }
    }

}
