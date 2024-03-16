using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 物体（角色）的Transform组件
    public float smoothSpeed = 0.125f; // 相机移动的平滑度
    public Vector3 offset = new Vector3(0f, 5f, 0f); // 相机与物体的偏移量

    public float minY = 1f; // Y坐标的最小值
    public float maxY = 52f; // Y坐标的最大值

    void Start()
    {
        // 设置相机的初始位置，并确保在合法范围内
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, minY, transform.position.z);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 计算相机的目标位置
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.x = transform.position.x; // 锁定 X 轴
            desiredPosition.z = transform.position.z; // 锁定 Z 轴

            // 限制 Y 轴的范围
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

            // 使用 SmoothDamp 实现相机的平滑移动
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
