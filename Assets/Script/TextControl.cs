using System.Collections;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Collider2D trigger1;
    public Collider2D trigger2;
    public Collider2D trigger3;

    private bool showMoveText = true;
    private bool showInteractText = false;
    private bool showControlText = false;
    private bool showSailText = false;
    private bool showText = false;

    void Start()
    {
        // 显示 "Press A and D To Move" 文字
        StartCoroutine(ShowText("Press A and D To Move"));

        // 开始监听按键事件
        StartCoroutine(CheckInput());
    }

    IEnumerator CheckInput()
    {
        while (true)
        {
            // 当按下I时，显示 "Use Space To Control" 文字
            if (Input.GetKeyDown(KeyCode.I) && !showControlText)
            {
                showControlText = true;
                StartCoroutine(ShowText("Use Space To Control"));
            }

            yield return null;
        }
    }

    IEnumerator ShowText(string message)
    {
        textMeshPro.text = message;
        textMeshPro.enabled = true;
        yield return new WaitForSeconds(3f); // 停留两秒
        textMeshPro.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 碰撞到collider trigger 1时，显示 "Press I To Interact" 文字
        if (other == trigger1 && !showInteractText)
        {
            showInteractText = true;
            StartCoroutine(ShowText("Press I To Interact"));
        }
        // 碰撞到collider trigger 2时，显示 "Press S To Sail" 文字
        else if (other == trigger2 && !showSailText)
        {
            showSailText = true;
            StartCoroutine(ShowText("Press S To Sail"));
        }

        if (other == trigger3 && !showText)
        {
            showText = true;
            StartCoroutine(ShowText("You have finished the tutorial"));
        }
    }
}
