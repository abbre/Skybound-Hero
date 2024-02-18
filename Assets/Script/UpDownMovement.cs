using UnityEngine;
using System.Collections;

public class UpDownMovement : MonoBehaviour
{
    private Vector2 originalPosition;
    private SpriteRenderer spriteRenderer;
    private bool movingUp = false;
    private bool movingDown = false;

    void Start()
    {
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 启动协程
        StartCoroutine(UpDownRoutine());
    }

    IEnumerator UpDownRoutine()
    {
        while (true)
        {
            // 缓慢向上移动
            movingUp = true;
            while (transform.position.y < originalPosition.y + 5f)
            {
                transform.Translate(Vector2.up * Time.deltaTime * 2f);
                yield return null;
            }
            movingUp = false;

            // 停留一段时间
            yield return new WaitForSeconds(1f);

            // 缓慢向下移动
            movingDown = true;
            while (transform.position.y > originalPosition.y)
            {
                transform.Translate(Vector2.down * Time.deltaTime * 3f);
                yield return null;
            }
            movingDown = false;

            // 停留一段时间
            yield return new WaitForSeconds(2f);
        }
    }

    // 设置 sprite 的朝向
    void SetSpriteDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
