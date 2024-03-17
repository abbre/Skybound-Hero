using UnityEngine;
using UnityEngine.UI;


public class FlashBomb : MonoBehaviour
{
    private float _timer;
    private float _alphaValue = 1f;
    private float _flashTimeCnter = 0f;

    private Image image;

    private bool _inFlash = false;
    [SerializeField] private float flashFrequency;
    [SerializeField] private float flashOpacity;

    public Cameras camera;

    public AudioSource audioSource;
    public AudioClip thunder;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        image.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!camera.enableFlash) return;
        _timer += Time.deltaTime;
        if (_timer >= flashFrequency)
        {
            _inFlash = true;
            if (_inFlash)
            {
                audioSource.clip = thunder;
                audioSource.loop = false;
                audioSource.Play();
                Flash();
            }
        }
    }

    private void Flash()
    {
        _flashTimeCnter += Time.deltaTime;
        if (_flashTimeCnter < 1.0f) //白屏
        {
            _alphaValue = flashOpacity;
        }
        else if (_flashTimeCnter < 1.2f) //灭掉
        {
            _alphaValue = 0f;
        }
        else if (_flashTimeCnter < 1.4f) //闪一下
        {
            _alphaValue = flashOpacity;
        }
        else //灭掉
        {
            _alphaValue -= 0.05f;
            if (_alphaValue <= 0f)
            {
                _inFlash = false;
                _flashTimeCnter = 0f;
                _timer = 0f;
            }
        }

        image.color = new Color(1, 1, 1, _alphaValue);
    }
}