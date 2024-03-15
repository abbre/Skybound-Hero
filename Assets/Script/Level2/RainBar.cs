using UnityEngine;
using UnityEngine.UI;

public class RainBar : MonoBehaviour
{
    public GameObject rainBar;
    public Slider rainSlider;
    public Image Fill;
    [SerializeField] private float healthDecline = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rainSlider.value = rainSlider.maxValue;
    }




    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Rain"))
        {
            rainSlider.value-= healthDecline;
            rainSlider.value = Mathf.Clamp(rainSlider.value, rainSlider.minValue, rainSlider.maxValue);

            if (rainSlider.value > rainSlider.maxValue / 2)
            {
                Fill.color = Color.white;
            }else if (rainSlider.value <= rainSlider.maxValue / 2 && rainSlider.value >= rainSlider.maxValue / 3)
            {
                Fill.color = new Color(1.0f, 0.64f, 0.0f);
            }else
            {
                Fill.color = Color.red;
            }
        }
    }
}