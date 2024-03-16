using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cameras : MonoBehaviour
{
    public Camera[] cameras;
    public Transform[] respawnPoints;
    public Transform[] endPoints;
    private int currentCameraIndex = 0;

    public PlaneController planeController;
    public Rigidbody2D planeRigidbody;
    private Quaternion initialRotation;

    public RainBar rainBar;
    public GameObject bird;

    public GameObject whiteScreen;
    public GameObject rain;
    public Image introImage; // 引入的图片
    public Image endImage;
    public TextMeshProUGUI endText;

    private float startTime;
    private float endTime;

    private void Start()
    {
        initialRotation = transform.rotation;
        InitializeCameras();
        StartCoroutine(IntroSequence()); // 开始引入序列
        startTime = Time.time; // 记录游戏开始时间
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
                    if (currentCameraIndex == 6)
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

    private System.Collections.IEnumerator WhiteScreenTransition(float duration)
    {
        if (whiteScreen != null)
        {
            // Enable the white screen
            whiteScreen.SetActive(true);

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
            // Fade out
            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timer / (duration * 0.5f));
                SetScreenAlpha(whiteScreen, alpha);
                yield return null;
            }
            StartCoroutine(MoveBird(5f));
            
            // Disable the white screen
            whiteScreen.SetActive(false);
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

    private System.Collections.IEnumerator MoveBird(float duration)
    {
        if (bird != null)
        {
            // Target position for the bird
            Vector3 targetPosition = new Vector3(455f, bird.transform.position.y, bird.transform.position.z);

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

            Time.timeScale = 0f;
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}