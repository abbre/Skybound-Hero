using UnityEngine;
using System.Collections;

public class UpDownMovement : MonoBehaviour
{
    private Vector2 originalPosition;
    private bool movingUp = false;
    private bool movingDown = false;

    void Start()
    {
        originalPosition = transform.position;
    
        // 启动协程
        StartCoroutine(UpDownRoutine());
    }

    IEnumerator UpDownRoutine()
    {
        while (true)
        {
            movingUp = true;
            while (transform.position.y < originalPosition.y + 0.25f)
            {
                transform.Translate(Vector2.up * Time.deltaTime * 1f);
                yield return null;
            }
            movingUp = false;

            movingDown = true;
            while (transform.position.y > originalPosition.y)
            {
                transform.Translate(Vector2.down * Time.deltaTime * 1f);
                yield return null;
            }
            movingDown = false;
        }
    }
}
