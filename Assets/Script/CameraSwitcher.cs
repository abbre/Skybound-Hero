using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public Camera camera2;
    public Camera camera3;
    public Camera camera4;

    private bool isPlane = false;
    private bool goCamera3 = false;
    private bool goCamera4 = false;

    public Transform respawnPointCamera3;
    public Transform respawnPointCamera4;
    

    public Collider2D endZoneCamera3;
    

    void Start()
    {
        // 初始时只激活 Player 相关相机
        SwitchToCamera1();
    }

    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (transform.position.x < -144f)
            {
                SwitchToCamera1();
            }
            else if (transform.position.x >= -144f && transform.position.x <= -111.5f)
            {
                SwitchToCamera2();
            }
            else
            {
                SwitchToCamera3();
            }
        }
        else if (gameObject.CompareTag("Plane"))
        {
            isPlane = true;

            if (goCamera3)
            {
                SwitchToCamera3();
            }
            if (goCamera4)
            {
                SwitchToCamera4();
            }
        }
    }

    void SwitchToCamera1()
    {
        camera1.Priority = 1;
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
        camera4.gameObject.SetActive(false); // 确保在切换相机时都将其关闭
        Debug.Log("Switched to Camera 1");
    }

    void SwitchToCamera2()
    {
        camera1.Priority = 0;
        camera2.gameObject.SetActive(true);
        camera3.gameObject.SetActive(false);
        camera4.gameObject.SetActive(false);
        Debug.Log("Switched to Camera 2");
    }

    void SwitchToCamera3()
    {
        camera1.Priority = 0;
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(true);
        camera4.gameObject.SetActive(false);
        Debug.Log("Switched to Camera 3");
        goCamera3 = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlane)
        {
            if (other.CompareTag("Object"))
            {
                if (goCamera3)
                {
                    transform.position = respawnPointCamera3.position;
                }
                else if (goCamera4)
                {
                    transform.position = respawnPointCamera4.position;
                }
                // Add more conditions for other cameras if needed
            }

            if (other == endZoneCamera3)
            {
                SwitchToCamera4();
                transform.position = respawnPointCamera4.position;
                goCamera3 = false;
                goCamera4 = true;
            }
        }
    }

    void SwitchToCamera4()
    {
        camera1.Priority = 0;
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
        camera4.gameObject.SetActive(true);
        Debug.Log("Switched to Camera 4");
    }
}
