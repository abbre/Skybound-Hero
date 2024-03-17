using System.Collections;
using UnityEngine;

public class Level2Cameras : MonoBehaviour
{
    public Camera[] cameras;
    public Transform[] respawnPoints;
    public Collider2D[] endPoints; // 修改为 Collider2D 数组

    private int currentCameraIndex = 0;
    public PlaneController planeController;
    public Rigidbody2D planeRigidbody;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        InitializeCameras();
    }

    private void InitializeCameras()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = false; // 初始化所有相机为禁用状态
        }

        cameras[currentCameraIndex].enabled = true; // 启用第一个相机
    }

    private void SwitchToNextCamera()
    {
        StartCoroutine(TransitionToCamera());
    }

    private IEnumerator TransitionToCamera()
    {
        // 禁用当前相机
        cameras[currentCameraIndex].enabled = false;

        // 更新相机索引
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // 传送玩家到下一个重生点
        transform.position = respawnPoints[currentCameraIndex].position;

        // 启用下一个相机
        cameras[currentCameraIndex].enabled = true;

        // 延迟执行
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Object") || other.CompareTag("Ground"))
        {
            // 检测到触发器，传送回当前相机的重生点
            transform.position = respawnPoints[currentCameraIndex].position;
            planeController.isSpiraling = false;
            planeRigidbody.gravityScale = 1f;
            planeRigidbody.velocity = Vector2.zero;
            planeRigidbody.angularVelocity = 0f;
            transform.rotation = initialRotation;
        }
        else
        {
            // 检测到其他标签的触发器
            for (int i = 0; i < endPoints.Length; i++)
            {
                if (other == endPoints[currentCameraIndex]) // 检测与当前相机绑定的触发器
                {
                    // 传送到相应相机的终点
                    transform.position = endPoints[currentCameraIndex].transform.position;
                    SwitchToNextCamera();
                }
                break; // 跳出循环，避免重复处理
            }
        }
    }
}
