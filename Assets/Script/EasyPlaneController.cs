using UnityEngine;

public class EasyPlaneController : MonoBehaviour
{
    public float maxFlyingForce = 20f;
    private float currentFlyingForce = 0f;
    private bool grounded = false;
    private bool inWater = false;
    public GameObject childObject; // 子物体的引用
    public Sprite newSprite; // 要更换的 Sprite
    public Sprite defaultSprite;

    public GameObject wave;
    private WaveMovement waveMovement;
    public GameObject waste; // 垃圾的预制体
    private bool wastecount;

    private Rigidbody2D rb;
    public Player player; 
    public PlayerHealthManager playerHealthManager;


    public void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        waveMovement = wave.GetComponent<WaveMovement>(); // 获取 Wave 的移动脚本
        waveMovement.enabled = false; 
        wastecount = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Object") || (other.CompareTag("Ground") && other.CompareTag("Water")))
        {
            if (other.CompareTag("Ground"))
            {
                playerHealthManager.TakeDamage();
            }
            grounded = true;
            currentFlyingForce = 0f;
            Debug.Log("Grounded or Object Triggered!");
            rb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            
            SwitchToPlayerControl();
        }
        else if (other.CompareTag("Water"))
        {
            inWater = true;
            waveMovement.enabled = true;
        }
    }
    void SwitchToPlayerControl()
    {   
        Debug.Log("Switching to Player control...");
        // 禁用 EasyPlaneController 脚本
        this.enabled = false;

        // 启用 Player 脚本
        player.enabled = true;
        player.Start();

        // 重置 EasyPlaneController 的状态
        grounded = false;
        inWater = false;
        currentFlyingForce = 0f;

        transform.rotation = Quaternion.identity;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 确保角色可以移动
        rb.velocity = Vector2.zero;

        SpriteRenderer childRenderer = childObject.GetComponent<SpriteRenderer>();
        if (childRenderer != null && defaultSprite != null)
        {
            childRenderer.sprite = defaultSprite;
        }
        if (wastecount == true)
        {
            DropWaste();
        }

        if (inWater)
        {
            waveMovement.enabled = true;
        }
        Debug.Log("Player control enabled.");
    }
    void DropWaste()
    {
        if (waste != null)
        {
            // 将垃圾的位置设置为玩家的位置
            waste.transform.position = transform.position;

            // 获取垃圾对象的刚体组件
            Rigidbody2D wasteRb = waste.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("waste is null!");
        }
        wastecount = false;
    }

}