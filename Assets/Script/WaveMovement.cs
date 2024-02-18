using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    public float speed = 5f; // 移动速度

    void Update()
    {
        // 每帧向右移动一定距离
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
