using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    // Update is called once per frame
    void Update()
    {
        // 检查玩家的位置，根据位置切换相机
        if (transform.position.x < -144f)
        {
            SwitchToCamera1();
        }
        else
        {
            SwitchToCamera2();
        }
    }

    void SwitchToCamera1()
    {
        camera1.Priority = 1;
        camera2.Priority = 0;
    }

    void SwitchToCamera2()
    {
        camera1.Priority = 0;
        camera2.Priority = 1;
    }
}