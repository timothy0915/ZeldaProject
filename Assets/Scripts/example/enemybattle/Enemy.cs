using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public FloatValue maxHealth;  // 這是 Scriptable Object，存最大血量
    private float currentHealth;  // **本地變數，實際運行時的血量**

    private void Start()
    {
        currentHealth = maxHealth.initialValue;  // 初始化血量
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} 受到了 {damage} 傷害，剩餘血量: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 被擊敗！");
        Destroy(gameObject);  // 銷毀敵人
    }
}
