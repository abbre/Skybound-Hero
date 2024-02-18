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
    private bool canControl; // 新增控制方式的开关
    private bool hasAppliedForce; // 检查是否已经施加过力
    public GameObject childObject; // 子物体的引用
    public Sprite newSprite; // 要更换的 Sprite

    private Rigidbody2D rb;
    public Animator anim;

    public EasyPlaneController easyPlaneController; // 引用另一个脚本

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        canControl = true; // 初始化为可以控制
    }

    void Update()
    {
        if (canControl)
        {
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
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

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
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

                // 遍历碰撞物体
                foreach (Collider2D collider in colliders)
                {
                    // 检查物体是否具有 "plane" 标签
                    if (collider.CompareTag("Plane"))
                    {
                        // 销毁碰撞到的物体
                        Destroy(collider.gameObject);
                        // 禁用 walk 和 hurt 动画
                        anim.SetBool("isMoving", false);
                        anim.SetBool("isHurt", false);
                        canControl = false; // 切换控制方式
                        break; // 停止遍历，只处理一个碰撞到的物体
                    }
                }
            }
        }
        else if (!hasAppliedForce && Input.GetKey(KeyCode.Space)) // 检查是否已经施加过力
        {
            // 如果按下空格键，施加向右的力
            rb.AddForce(Vector2.right * 15f, ForceMode2D.Impulse);
            hasAppliedForce = true; // 设置标志位为 true，表示已经施加过力
            // 切换脚本
            easyPlaneController.enabled = true;
            this.enabled = false; // 禁用当前脚本
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
            if (childRenderer != null && newSprite != null)
            {
                childRenderer.sprite = newSprite;
            }
        }
    }
}
