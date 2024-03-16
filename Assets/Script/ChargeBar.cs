using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public PlaneController planeController;
    public Slider chargeBar;
    public GameObject barGameObj;

    void Start()
    {
        barGameObj.SetActive(false); // 初始时消失
        chargeBar.value = 0;
        chargeBar.maxValue = planeController.holdingTimeThreshold;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            barGameObj.SetActive(true);
            chargeBar.value = planeController.holdingTimeCnter;
            chargeBar.value = Mathf.Clamp(chargeBar.value, chargeBar.minValue, chargeBar.maxValue);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
        {
            barGameObj.SetActive(false); // 松开空格键时消失
        }
    }
}