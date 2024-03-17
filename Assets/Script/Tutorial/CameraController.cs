using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class CameraController : MonoBehaviour
{
    [Header("Test")]
    public Camera[] cameras; // 摄像机数组
    public Transform[] respawnPoints; // 复活点数组
    [HideInInspector]public int currentCameraIndex = 0; // 当前摄像机索引
    [Space(10)]
    [Tooltip("This is Respawn")]
    private bool respawn = false; // 标记是否需要显示第一个红色区域
    public GameObject firstRedArea; // 第一个红色区域对象
    public Image blackoutScreen;
    [HideInInspector] public bool gameStart = false;
    

    private void Start()
    {
        InitializeCameras();
        firstRedArea.SetActive(false); // 初始时禁用第一个红色区域
        Invoke("ShowFirstRedArea", 10f); // 10秒后尝试显示第一个红色区域
    }

    private void InitializeCameras()
    {
        // 除了第一个摄像机外，其余摄像机都禁用
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = i == 0;
        }
    }

    private void ShowFirstRedArea()
    {
        firstRedArea.SetActive(true);
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
        transform.position = respawnPoints[currentCameraIndex].position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 处理玩家触碰到边界或红色区域的逻辑
        if (other.CompareTag("Bound"))
        {
            transform.position = respawnPoints[currentCameraIndex].position;
        }
        else if (other.CompareTag("RedArea"))
        {
            if (currentCameraIndex == 3) // 假设Camera4的索引为3
            {
                StartCoroutine(SequenceCameraChange());
            }
            else if (currentCameraIndex < 3)
            {
                SwitchToNextCamera();
            }
        }
    }
    private IEnumerator SequenceCameraChange()
    {
        // Camera4 到 Camera5 的变换
        yield return StartCoroutine(FadeToBlack(0.1f)); // 淡出为黑屏，持续2秒
        SwitchCamera(4); // 切换到 Camera5
        yield return StartCoroutine(FadeFromBlack(2f)); // 从黑屏淡入，持续2秒
        
        // Camera5 到 Camera6 的变换
        yield return new WaitForSeconds(1.5f); // 在 Camera5 停留
        yield return StartCoroutine(FadeToBlack(1f)); // 淡出为黑屏，持续1秒
        SwitchCamera(5); // 切换到 Camera6
        yield return StartCoroutine(FadeFromBlack(1f)); // 从黑屏淡入，持续1秒
        
        // Camera6 到 Camera7 的变换
        yield return new WaitForSeconds(5f); // 在 Camera6 停留
        yield return StartCoroutine(FadeToBlack(1f)); // 淡出为黑屏，持续1秒
        SwitchCamera(6); // 切换到 Camera7
        yield return StartCoroutine(FadeFromBlack(1f)); // 从黑屏淡入，持续1秒
        
        yield return new WaitForSeconds(5f); // 在 Camera7 停留

        SceneManager.LoadScene("Level1");
 

    }

    private void SwitchCamera(int cameraIndex)
    {
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }
        cameras[cameraIndex].enabled = true;
    }

    private IEnumerator FadeToBlack(float duration)
    {
        float counter = 0;
        while (counter < duration)
        {
            gameStart = true;
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            blackoutScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            blackoutScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}