using UnityEngine;
using UnityEngine.UI; // 使用UI命名空间

public class CameraController : MonoBehaviour
{
    public Camera[] cameras; // 摄像机数组
    public Transform[] respawnPoints; // 复活点数组
    public GameObject player;
    public Button switchButton;
    private int currentCameraIndex = 0;

    private void Start()
    {
        InitializeCameras();
        switchButton.gameObject.SetActive(false); // 初始时隐藏按钮
        Invoke("ShowButton", 10f); // 10秒后显示按钮
    }
    private void InitializeCameras()
    {
        // 除了第一个摄像机外，其余摄像机都禁用
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = i == 0;
        }
    }

    private void ShowButton()
    {
        switchButton.gameObject.SetActive(true);
        switchButton.onClick.AddListener(SwitchToNextCamera); // 给按钮添加点击事件
    }

    private void SwitchToNextCamera()
    {
        // 禁用当前摄像机
        cameras[currentCameraIndex].enabled = false;

        // 更新摄像机索引，循环到下一个摄像机
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // 启用下一个摄像机
        cameras[currentCameraIndex].enabled = true;

        // 传送玩家到下一个复活点
        player.transform.position = respawnPoints[currentCameraIndex].position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            if (other.tag == "Boundary")
            {
                // 玩家碰到边界，返回当前摄像机对应的复活点
                player.transform.position = respawnPoints[currentCameraIndex].position;
            }
            else if (other.tag == "RedArea")
            {
                // 玩家碰到红色区域，切换到下一个摄像机并传送到对应复活点
                SwitchToNextCamera();
            }
        }
    }
}
