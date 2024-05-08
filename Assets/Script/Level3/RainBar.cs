using UnityEngine;
using UnityEngine.UI;

public class RainBar : MonoBehaviour
{
    public GameObject rainBar;
    public Slider rainSlider;
    public Image Fill;
    [SerializeField] public float healthDecline = 0.045f;
    private bool _inRain = false;
    public Cameras camera;
    private float timer = 0f;
    public float lerpDuration = 0.5f; // Duration for the lerping effect

    // Start is called before the first frame update
    void Start()
    {
        rainSlider.value = rainSlider.maxValue;
    }

    void Update()
    {
        if (!camera.enableFlash) return;

        if (!_inRain)
        {
            timer += Time.deltaTime;
            float targetValue = rainSlider.value + healthDecline;
            rainSlider.value = Mathf.Lerp(rainSlider.value, targetValue, timer / lerpDuration);
        }
        else
        {
            timer += Time.deltaTime;
            float targetValue = rainSlider.value - healthDecline;
            rainSlider.value = Mathf.Lerp(rainSlider.value, targetValue, timer / lerpDuration);
        }

        timer = Mathf.Clamp(timer, 0f, lerpDuration); // Ensure timer doesn't exceed lerpDuration

        if (rainSlider.value > rainSlider.maxValue / 2)
        {
            Fill.color = Color.white;
        }
        else if (rainSlider.value <= rainSlider.maxValue / 2 && rainSlider.value >= rainSlider.maxValue / 3)
        {
            Fill.color = new Color(1.0f, 0.64f, 0.0f);
        }
        else
        {
            Fill.color = Color.red;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Rain"))
        {
            _inRain = true;
            timer = 0f; // Reset the timer when in rain
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Rain"))
        {
            _inRain = false;
            timer = 0f; // Reset the timer when exiting rain
        }
    }

    public void ResetRainBar()
    {
        rainSlider.value = rainSlider.maxValue;
        Fill.color = Color.white;
    }
}
