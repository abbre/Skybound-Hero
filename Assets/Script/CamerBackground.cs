using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerBackground : MonoBehaviour
{
    public Transform target;
    public Transform front, middle, far;
    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, 0f); // 只在 x 轴上移动
        far.position += new Vector3(amountToMove.x * 0.1f, 0f, 0f);
        middle.position += new Vector3(amountToMove.x * 0.18f, 0f, 0f); 
        front.position += new Vector3(amountToMove.x * 0.3f, 0f, 0f);

        lastPos = transform.position;
    }   
}
