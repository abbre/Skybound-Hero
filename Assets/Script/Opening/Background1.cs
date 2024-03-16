using UnityEngine;

public class Background1 : MonoBehaviour
{
    public float speed = 1f; // 背景1的移动速度

    void Update()
    {
        // 计算移动距离
        float movement = speed * Time.deltaTime;

        // 将移动应用于背景1的位置，这里假设背景1沿x轴移动
        transform.Translate(Vector3.right * movement);
    }
}