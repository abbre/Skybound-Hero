using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Collider2D pressI;
    private bool showInteractText = false;
    public GameObject arrow;
    public Image blackImage;

    public Player player; 
    public GameObject popupImage;



    void Start()
    {
        popupImage.SetActive(false);
        blackImage.gameObject.SetActive(false);
        HideText();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == pressI && !showInteractText)
        {
            // 当玩家角色进入触发器时显示文字，并且只显示一次
            showInteractText = true;
        }
    }

    void Update()
    {
        if (showInteractText && Input.GetKeyDown(KeyCode.I))
        {   
            // 当按下 "i" 键时隐藏文字
            StartCoroutine(PopupImage());
            arrow.SetActive(false);
        }
    }

    IEnumerator ShowText(string message)
    {
        textMeshPro.text = message;
        textMeshPro.enabled = true;
        yield return new WaitForSeconds(3f); // 停留三秒
        HideText();
    }

    void HideText()
    {
        textMeshPro.enabled = false;
        showInteractText = false;
    }

    IEnumerator PopupImage()
    {
        textMeshPro.text = "A letter: To my grandparents";
        textMeshPro.enabled = true;

        popupImage.SetActive(true);
        blackImage.gameObject.SetActive(true);
        blackImage.color = new Color(0f, 0f, 0f, 0.5f); // 设置遮罩的透明度

        // 放大图片
        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 originalScale = popupImage.transform.localScale;
        Vector3 targetScale = new Vector3(5f, 5f, 5f);
        Vector3 originalTextPos = textMeshPro.rectTransform.localPosition;
        Vector3 targetTextPos = originalTextPos + new Vector3(0f, 20f, 0f);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            popupImage.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            textMeshPro.rectTransform.localPosition = Vector3.Lerp(originalTextPos, targetTextPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 显示文字
        
        yield return new WaitForSeconds(1f); // 显示三秒钟

        // 隐藏图片、文字和遮罩，回到正常游戏
        HideText();
        popupImage.SetActive(false);
        blackImage.gameObject.SetActive(false);
    }
}