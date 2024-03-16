using UnityEngine;

public class MovingCloud : MonoBehaviour
{
    public float speed = 1f; // 控制水平移动速度

    void Update()
    {
        // 计算水平移动距离
        float horizontalMovement = speed * Time.deltaTime;

        // 将移动应用于云的位置，仅在水平方向上
        transform.Translate(Vector3.right * horizontalMovement);
    }
}