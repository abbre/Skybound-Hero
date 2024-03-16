using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Cameras: MonoBehaviour
{
    public Camera[] cameras; // Camera 数组声明
    
    private int currentCameraIndex = 0; // 当前摄像机索引

    void Start()
    {
        // 确保至少有一个摄像机
        if (cameras.Length > 0)
        {
            // 禁用所有摄像机，除了第一个
            for (int i = 1; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Camera array is empty!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CameraTrigger"))
        {
            Debug.Log("hh");
            SwitchToNextCamera();
        }
    }

    private void SwitchToNextCamera()
    {
        // 禁用当前摄像机
        cameras[currentCameraIndex].gameObject.SetActive(false);
        
        // 增加摄像机索引，并确保在有效范围内
        currentCameraIndex++;
        currentCameraIndex = Mathf.Clamp(currentCameraIndex, 0, cameras.Length);

        // 启用下一个摄像机
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}