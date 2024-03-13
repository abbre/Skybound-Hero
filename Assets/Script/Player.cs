using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving;
    private bool canControl = true; 
    private float pressTime; 
    
    private Rigidbody2D rb;
    public Animator anim;
    
    public GameObject plane; 
    public Collider2D planeFly; 
    private bool isInsideTrigger = false;
   
    public PlaneController planeController; 
    public CameraSwitcher cameraSwitcher; 
    public GameObject childObject;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        plane.SetActive(false); // 隐藏飞机

        planeController.enabled = false;
    }

    void Update()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (canControl)
        {   
            StartChildAnimation();

            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

            isMoving = (horizontalInput != 0);
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
                pressTime = Time.time;
                anim.SetBool("isFlying", true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("isFlying", false);
            }

            if (Input.GetKey(KeyCode.Space) && Time.time - pressTime >= 2.5f && isInsideTrigger)
            {
                StartFlying();
            }

        }
    }

    void StartFlying()
    {
        canControl = false;
        planeController.enabled = true;
        this.enabled = false;  
        cameraSwitcher.enabled = false;
        plane.SetActive(true); // 显示飞机
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == planeFly)
        {
            isInsideTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == planeFly)
        {
            isInsideTrigger = false;
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
}
