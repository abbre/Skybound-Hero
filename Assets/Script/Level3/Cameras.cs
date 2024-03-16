using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        initialRotation = transform.rotation;
        InitializeCameras();
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
                    SwitchToNextCamera();
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
}
