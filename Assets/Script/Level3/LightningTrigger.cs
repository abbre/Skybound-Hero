using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningTrigger : MonoBehaviour
{
    [HideInInspector] public bool isLightning = false;
    [HideInInspector] public int randomNum = 0;

    [HideInInspector] public float value = 0.5f; // 保存原始颜色的HSV分量
    private float _prevValue = 0.5f;

    [SerializeField] float _valueSpeed = 0.1f;

    //public <List>LightningBG lightningbg;
    public List<LightningBG> lightningBgs = new List<LightningBG>();

    

    // Update is called once per frame
    private void Start()
    {
        GameObject[] bgList = GameObject.FindGameObjectsWithTag("lightning_bg");
        foreach (var bg in bgList)
        {
            lightningBgs.Add(bg.GetComponent<LightningBG>());
        }
    }

    void Update()
    {
  
        randomNum = Random.Range(1, 100);
        
        Debug.Log(isLightning);

        if (randomNum == 2 && !isLightning)
        {
            isLightning = true;
        }

        if (isLightning)
        {
            value += _valueSpeed;
            if (value >= 1.0f || value < 0.5f)
            {
                _valueSpeed = -_valueSpeed;
            }

            if (_prevValue > 0.55f && value <= 0.55f)
            {
                isLightning = false;
                value = 0.5f;
            }

            _prevValue = value;

            //lightningbg.Lightning();
            foreach (var lightningBg in lightningBgs)
            {
                lightningBg.Lightning();
            }
        }
    }
}