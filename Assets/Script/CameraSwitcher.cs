using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public Camera Camera1;
    public Camera Camera2;
    public Camera camera3;
    public Camera camera4;
    public Camera camera5;

    private bool isPlane = false;
    private bool hit = false;
    private bool goCamera3 = false;
    private bool goCamera4 = false;
    private bool goCamera5 = false;

    public Transform respawnPointCamera3;
    public Transform respawnPointCamera4;
    public Transform respawnPointCamera5;
    
    public Collider2D endZoneCamera2;
    public Collider2D endZoneCamera3;
    public Collider2D endZoneCamera4;
    
    public Rigidbody2D planeRigidbody;
    private Quaternion initialRotation;

    public PlaneController planeController;

    void Start()
    {
        // 初始时只激活 Player 相关相机
        SwitchToCamera1();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (transform.position.x < -144f)
            {
                SwitchToCamera1();
            }
            else if (transform.position.x >= -144f && !hit)
            {
                SwitchToCamera2();
            }
            else if (hit)
            {
                SwitchToCamera3();
            }
        }
        else if (gameObject.CompareTag("Plane"))
        {
            isPlane = true;
        }
    }

    void SwitchToCamera1()
    {
        camera1.Priority = 1;
        camera2.Priority = 0;
        camera3.gameObject.SetActive(false);
        camera4.gameObject.SetActive(false);
        camera5.gameObject.SetActive(false);
    }

    void SwitchToCamera2()
    {
        camera1.Priority = 0;
        camera2.Priority = 1;
    }

    void SwitchToCamera3()
    {
        camera1.enabled = false;
        camera2.enabled = false;
        Camera1.enabled = false;
        Camera2.enabled = false;
        camera1.Priority = 0;
        camera2.Priority = 0;
        camera3.gameObject.SetActive(true);
        goCamera3 = true;
    }

    void SwitchToCamera4()
    {
        camera3.gameObject.SetActive(false);
        camera4.gameObject.SetActive(true);
        goCamera4 = true;
        goCamera3 = false;
    }

    void SwitchToCamera5()
    {
        camera4.gameObject.SetActive(false);
        camera5.gameObject.SetActive(true);
        goCamera5 = true;
        goCamera4 = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == endZoneCamera2)
            {
                SwitchToCamera3();
            }

        if (isPlane)
        {
            if (other.CompareTag("Object") || other.CompareTag("Ground"))
            {
                if (camera3.gameObject.activeSelf)
                {
                    transform.position = respawnPointCamera3.position;
                }
                else if (camera4.gameObject.activeSelf)
                {
                    transform.position = respawnPointCamera4.position;
                }
                else if (camera5.gameObject.activeSelf)
                {
                    transform.position = respawnPointCamera5.position;
                }

                planeController.isSpiraling = false;
                planeRigidbody.gravityScale = 1f; 
                planeRigidbody.velocity = Vector2.zero;
                planeRigidbody.angularVelocity = 0f;
                transform.rotation = initialRotation;
                // Add more conditions for other cameras if needed
            }

            if (other == endZoneCamera3)
            {
                SwitchToCamera4();
            }
            if (other == endZoneCamera4)
            {
                SwitchToCamera5();
            }
        }
    }
}