using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Cameras : MonoBehaviour
{
    public Camera[] cameras;
    public Transform[] respawnPoints;
    public Transform[] endPoints;
    [SerializeField] private int currentCameraIndex = 0;

    public PlaneController planeController;
    public Rigidbody2D planeRigidbody;
    private Quaternion initialRotation;

    public RainBar rainBar;
    public GameObject bird;

    public GameObject whiteScreen;
    public GameObject rain;
    public Image introImage; // 引入的图片
    public Image endImage;
    public Image image1; // 第一张图片
    public Image image2; // 第二张图片
    public TextMeshProUGUI endText;

    public AudioClip rainSound;
    public AudioClip thunderSound;
    public AudioClip bgm;

    private AudioSource audioSource;

    private float startTime;
    private float endTime;

    public bool enableFlash = true;

    public GameObject SceneChangeButton;

    private void Start()
    {
        initialRotation = transform.rotation;
        InitializeCameras();
        StartCoroutine(IntroSequence()); // 开始引入序列
        startTime = Time.time; // 记录游戏开始时间

        audioSource = GetComponent<AudioSource>();

        // 播放雨声
        audioSource.clip = rainSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        
        if (rainBar.rainSlider.value == 0)
        {
            transform.position = respawnPoints[currentCameraIndex].position;

            planeController.isSpiraling = false;
            planeRigidbody.gravityScale = 1f;
            planeRigidbody.velocity = Vector2.zero;
            planeRigidbody.angularVelocity = 0f;
            transform.rotation = initialRotation;

            rainBar.ResetRainBar();
        }
    }

    private void InitializeCameras()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = false; // Initialize all cameras as disabled
        }

        cameras[currentCameraIndex].enabled = true; // Enable the first camera
    }

    private void SwitchToNextCamera()
    {
        if (currentCameraIndex == 3)
        {
            StartCoroutine(WhiteScreenTransition(1f));
        }
        else
        {
            StartCoroutine(TransitionToCamera());
        }
    }

    private System.Collections.IEnumerator TransitionToCamera()
    {
        // Disable current camera
        cameras[currentCameraIndex].enabled = false;

        // Update camera index
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Teleport player to next respawn point
        transform.position = respawnPoints[currentCameraIndex].position;

        // Enable next camera
        cameras[currentCameraIndex].enabled = true;

        rainBar.ResetRainBar();

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Object") || other.CompareTag("Ground"))
        {
            transform.position = respawnPoints[currentCameraIndex].position;

            planeController.isSpiraling = false;
            planeRigidbody.gravityScale = 1f;
            planeRigidbody.velocity = Vector2.zero;
            planeRigidbody.angularVelocity = 0f;
            transform.rotation = initialRotation;

            rainBar.ResetRainBar();
        }
        else
        {
            // Check if the triggered collider is in the endPoints array
            for (int i = 0; i < endPoints.Length; i++)
            {
                if (other.transform == endPoints[currentCameraIndex])
                {
                    // Teleport player to the corresponding end point for the current camera
                    transform.position = endPoints[currentCameraIndex].position;
                    if (currentCameraIndex == 8)
                    {
                        endTime = Time.time; // 记录游戏结束时间
                        StartCoroutine(FinalSequence());
                    }
                    else
                    {
                        SwitchToNextCamera();
                    }
                    break;
                }
            }
        }
    }

    private IEnumerator WhiteScreenTransition(float duration)
    {
        if (whiteScreen != null)
        {
            // Enable the white screen
            whiteScreen.SetActive(true);

            audioSource.clip = thunderSound;
            audioSource.loop = false;
            audioSource.Play();

            if (rain != null)
            {
                rain.SetActive(false);
            }

            // Fade in
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, timer / (duration * 0.1f));
                SetScreenAlpha(whiteScreen, alpha);
                yield return null;
            }

            // Wait for the white screen to stay fully visible
            yield return new WaitForSeconds(3f);
            
            StartCoroutine(TransitionToCamera());
            enableFlash = false;
            // Fade out
            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / (duration * 0.5f));
                SetScreenAlpha(whiteScreen, alpha);
                yield return null;
            }
            StartCoroutine(MoveBird(6f));
            
            // Disable the white screen
            whiteScreen.SetActive(false);

            while (audioSource.isPlaying)
        {
            yield return null;
        }

            audioSource.clip = bgm; 
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void SetScreenAlpha(GameObject screen, float alpha)
    {
        if (screen != null)
        {
            Image screenImage = screen.GetComponent<Image>();
            if (screenImage != null)
            {
                Color color = screenImage.color;
                color.a = alpha;
                screenImage.color = color;
            }
        }
    }

    private IEnumerator MoveBird(float duration)
    {
        if (bird != null)
        {
            // Target position for the bird
            Vector3 targetPosition = new Vector3(398.2f, bird.transform.position.y, bird.transform.position.z);

            // Start position of the bird
            Vector3 startPosition = bird.transform.position;

            // Timer for lerping
            float timer = 0f;

            while (timer < duration)
            {
                // Update the timer
                timer += Time.deltaTime;

                // Calculate the progress
                float progress = timer / duration;

                // Move the bird towards the target position
                bird.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

                // Wait for the next frame
                yield return null;
            }

            // Ensure the bird reaches the target position exactly
            bird.transform.position = targetPosition;
        }
        else
        {
            Debug.LogWarning("Bird GameObject is not assigned in the Cameras script.");
        }
    }

    private IEnumerator IntroSequence()
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

    private IEnumerator FinalSequence()
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
            else
            {
                Debug.LogWarning("endText is not found!");
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

            yield return new WaitForSeconds(1f);

            // Show Image 1
            if (image1 != null)
            {
                image1.gameObject.SetActive(true);
                timer = 0f;
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    float alpha = Mathf.Lerp(0f, 1f, timer / (duration * 0.25f));
                    SetImageAlpha(image1, alpha);
                    yield return null;
                }

                yield return new WaitForSeconds(2f);
            }

            // Show Image 2
            if (image2 != null)
            {
                image2.gameObject.SetActive(true);
                timer = 0f;
                SceneChangeButton.SetActive(true);
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    float alpha = Mathf.Lerp(0f, 1f, timer / (duration * 0.25f));
                    SetImageAlpha(image2, alpha);
                    yield return null;
                }

                yield return new WaitForSeconds(2f);

            }
            
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
