using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public PlaneController planeController;
    public Slider chargeBar;
    public GameObject barGameObj;
   
    void Start()
    {
       // barGameObj.SetActive(false); // 初始时消失
        chargeBar.value = 0;
        chargeBar.maxValue = planeController.holdingTimeThreshold;
    }

    void Update()
    {
        // 如果按下了空格键
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            //barGameObj.SetActive(true); // 按下空格键时显示
            chargeBar.value = planeController.holdingTimeCnter;
            chargeBar.value = Mathf.Clamp(chargeBar.value, chargeBar.minValue, chargeBar.maxValue);
        }
        else // 如果未按下空格键
        {
           // barGameObj.SetActive(false); // 松开空格键时消失
        }
    }
}