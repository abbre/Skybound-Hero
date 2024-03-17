using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level2Cameras : MonoBehaviour
{
    public Camera[] cameras;
    public Transform[] respawnPoints;
    public Collider2D[] endPoints; // 修改为 Collider2D 数组

    [SerializeField] private int currentCameraIndex = 0;
    public PlaneController planeController;
    public Rigidbody2D planeRigidbody;
    private Quaternion initialRotation;
    [Header("Test"),Tooltip("Testest")]
    public Image introImage; // 引入的图片
    public Image endImage;
    public TextMeshProUGUI endText;
    public GameObject SceneLoadButton;
    public AudioClip bgm;
    private AudioSource audioSource;

    private float startTime;
    private float endTime;

    void Start()
    {
        initialRotation = transform.rotation;
        InitializeCameras();

        StartCoroutine(IntroSequence()); // 开始引入序列
        startTime = Time.time; // 记录游戏开始时间

        audioSource = GetComponent<AudioSource>();

        // 播放雨声
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();
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
                    if (currentCameraIndex == 6)
                    {
                        endTime = Time.time; // 记录游戏结束时间
                        StartCoroutine(FinalSequence());
                    }
                    else
                    {
                        SwitchToNextCamera();
                    }
                }
                break; // 跳出循环，避免重复处理
            }
        }
    }
    private System.Collections.IEnumerator IntroSequence()
    {
        if (introImage != null)
        {
            // Enable the intro image
            introImage.gameObject.SetActive(true);

            // Fade in
            float duration = 2f;
            float timer = 0f;
            
            yield return new WaitForSeconds(2f);

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / (duration * 0.5f));
                SetImageAlpha(introImage, alpha);
                yield return null;
            }

            // Disable the intro image
            introImage.gameObject.SetActive(false);
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    private System.Collections.IEnumerator FinalSequence()
    {
        if (endImage != null)
        {
            // Enable the intro image
            endImage.gameObject.SetActive(true);
            if (endText != null)
            {
                endText.gameObject.SetActive(true);
                string timeText = FormatTime(endTime - startTime);
                endText.text = timeText;
            }

            // Fade in
            float duration = 2f;
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, timer / (duration * 0.25f));
                SetImageAlpha(endImage, alpha);
                yield return null;
            }
            SceneLoadButton.SetActive(true);
            yield return new WaitForSeconds(2f);
            //Time.timeScale = 0f;
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

