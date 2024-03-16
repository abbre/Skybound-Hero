using UnityEngine;

public class PropMovement : MonoBehaviour
{
    public float speed = 1f; // 控制上下移动的速度
    public float distanceFactor = 1f; // 控制移动距离的因子

    private Vector3 startPosition;

    void Start()
    {
        // 记录道具的初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 计算上下移动的距离，使用时间的正弦值乘以距离因子
        float verticalMovement = Mathf.Sin(Time.time * speed) * distanceFactor;

        // 应用上下移动到道具的位置
        transform.position = startPosition + Vector3.up * verticalMovement;
    }
}