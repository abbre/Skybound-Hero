using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport2 : MonoBehaviour
{
    public float targetX = -151.1f;
    public float targetY = 24.25f;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 确认碰撞的对象是否为角色
        if (other.CompareTag("Player"))
        {
            // 获取角色当前位置
            Vector2 currentPosition = other.transform.position;
            // 设置传送目标位置
            Vector2 targetPosition = new Vector2(targetX, targetY);
            // 将角色传送到目标位置
            other.transform.position = targetPosition;
        }
    }
}
