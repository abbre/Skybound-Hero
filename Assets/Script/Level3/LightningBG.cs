using UnityEngine;
using UnityEngine.UI;

public class LightningBG : MonoBehaviour
{
    private SpriteRenderer lightningBackGround;
    public LightningTrigger lightningTrigger;


    // Start is called before the first frame update
    void Start()
    {
        lightningBackGround = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    public void Lightning()
    {
        lightningBackGround.color = Color.HSVToRGB(0, 0, lightningTrigger.value);
    }
}