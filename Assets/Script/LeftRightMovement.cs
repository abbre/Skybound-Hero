using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    private Vector2 originalPosition;
    private SpriteRenderer spriteRenderer;
    private bool movingLeft = false;
    private bool movingRight = false;

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
            movingRight = true;
            while (transform.position.x < originalPosition.x + 2f)
            {
                transform.Translate(Vector2.right * Time.deltaTime * 1f);
                yield return null;
            }
            movingRight = false;

            // 停留一段时间
            yield return new WaitForSeconds(2f);

            movingLeft = true;
            while (transform.position.x > originalPosition.x)
            {
                transform.Translate(Vector2.left * Time.deltaTime * 1f);
                yield return null;
            }
            movingLeft = false;

            // 停留一段时间
            yield return new WaitForSeconds(2f);
        }
    }
}
