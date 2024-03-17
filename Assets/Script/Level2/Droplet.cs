using UnityEngine;

public class Droplet : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float resetSpeed = 1f;
    public float resetDelay = 2.5f;

    private Vector3 originalPosition;
    private bool isFalling = false;

    void Start()
    {
        originalPosition = transform.position;
        StartFalling();
    }

    void Update()
    {
        if (isFalling)
        {
            // 计算新的位置
            Vector3 newPosition = transform.position + Vector3.down * fallSpeed * Time.deltaTime;

            // 更新位置
            transform.position = newPosition;

            // 如果到达目标位置
            if (transform.position.y <= -17f)
            {
                // 延迟重置位置
                Invoke("ResetPosition", resetDelay);
                isFalling = false;
            }
        }
    }

    void StartFalling()
    {
        isFalling = true;
    }

    void ResetPosition()
    {
        transform.position = originalPosition;
        Invoke("StartFalling", resetSpeed);
    }
}
