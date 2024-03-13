using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public Camera Camera1;
    public Camera Camera2;
    public Camera camera3;
    public Camera camera4;
    public Camera camera5;
    public Camera camera6;

    private bool isPlane = false;
    private bool hit = false;
    private bool goCamera3 = false;
    private bool goCamera4 = false;
    private bool goCamera5 = false;
    private bool goCamera6 = false;

    public Transform respawnPointCamera3;
    public Transform respawnPointCamera4;
    public Transform respawnPointCamera5;
    public Transform respawnPointCamera6;
    
    public Collider2D endZoneCamera2;
    public Collider2D endZoneCamera3;
    public Collider2D endZoneCamera4;
    public Collider2D endZoneCamera5;
    public Collider2D finish;
    
    public Rigidbody2D planeRigidbody;
    private Quaternion initialRotation;

    public PlaneController planeController;


    public Image blackScreen;
    public Image gameOverImage;
    public float fadeDuration = 1f;
    public float displayImageDuration = 2f;

    private bool isGameOver = false;

    void Start()
    {
        // 初始时只激活 Player 相关相机
        SwitchToCamera1();
        initialRotation = transform.rotation;

        blackScreen.gameObject.SetActive(false);
        gameOverImage.gameObject.SetActive(false);
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

    void SwitchToCamera6()
    {
        camera5.gameObject.SetActive(false);
        camera6.gameObject.SetActive(true);
        goCamera6 = true;
        goCamera5 = false;
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
                else if (camera6.gameObject.activeSelf)
                {
                    transform.position = respawnPointCamera6.position;
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
            if (other == endZoneCamera5)
            {
                SwitchToCamera6();
            }
            if (other == finish)
            {
                if (!IsGameOver())
                {
                    Time.timeScale = 0f;
                    ShowGameOverScreen();
                }
            }
        }
    }
    
    public void ShowGameOverScreen()
    {
        isGameOver = true;
        StartCoroutine(FadeOutAndShowImage());
    }

    IEnumerator FadeOutAndShowImage()
    {
        blackScreen.gameObject.SetActive(true);

        float timer = 0f;
        Color initialColor = blackScreen.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

        while (timer < fadeDuration)
        {
            float progress = timer / fadeDuration;
            blackScreen.color = Color.Lerp(initialColor, targetColor, progress);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(displayImageDuration);

        gameOverImage.gameObject.SetActive(true);

        while (Time.timeScale == 0f)
        {
            yield return null;
        }

        gameOverImage.gameObject.SetActive(false);
        blackScreen.color = initialColor;
        blackScreen.gameObject.SetActive(false);

        isGameOver = false;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

}