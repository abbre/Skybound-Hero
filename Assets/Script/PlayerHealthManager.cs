using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth = 3; // 最大生命值
    private int currentHealth; // 当前生命值
    public Image[] healthIcons; // 用于显示生命的图标
    private bool checkpoint;

    public GameObject triggerToShowHealth;
    public Transform respawn;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        checkpoint = false;
        foreach (Image icon in healthIcons)
        {
            icon.enabled = false;
        }
    }

    void UpdateHealthUI()
    {
        Debug.Log("Updating health UI...");
        if (checkpoint == true)
        {
            for (int i = 0; i < healthIcons.Length; i++)
            {
                if (i < currentHealth)
                {
                    healthIcons[i].enabled = true; // 显示生命图标
                }
                else
                {
                    healthIcons[i].enabled = false; // 隐藏生命图标
                }
            }

            if (currentHealth <= 0)
            {
                // 触发角色死亡逻辑，例如重置到最近的检查点
                RespawnAtCheckpoint();
            }
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        UpdateHealthUI();
    }

    public void RespawnAtCheckpoint()
    {   
        transform.position = respawn.position;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == triggerToShowHealth)
        {
            checkpoint = true;
            Debug.Log("Checkpoint triggered! checkpoint = " + checkpoint);
            UpdateHealthUI();
        }
    }
}
