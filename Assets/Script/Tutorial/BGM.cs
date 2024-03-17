using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource bgmManager;
    private AudioSource creditManager;
    public AudioClip tutorialBGM;
    public AudioClip getCredit;
    private float fadeSpeed = 0.05f; // 渐出速度
    private bool fadingOut = false; // 是否正在渐出

    public CameraController camera;
    public PlaneController planeController;

    // Start is called before the first frame update
    void Start()
    {
        bgmManager = gameObject.AddComponent<AudioSource>(); // 添加第一个 AudioSource 组件
        bgmManager.clip = tutorialBGM;
        bgmManager.loop = true;
        bgmManager.volume = 0.1f;
        bgmManager.Play();

        creditManager = gameObject.AddComponent<AudioSource>(); // 添加第二个 AudioSource 组件
        creditManager.clip = getCredit;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.gameStart && !fadingOut)
        {
            fadingOut = true;
            StartCoroutine(FadeOut());
        }

        if (planeController.playGetCredit)
        {
            creditManager.loop = false;
            creditManager.volume = 1.0f;
            creditManager.Play(); // 播放新的音频
            planeController.playGetCredit = false;
        }
    }

    IEnumerator FadeOut()
    {
        while (bgmManager.volume > 0)
        {
            bgmManager.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        bgmManager.Stop();
    }
}