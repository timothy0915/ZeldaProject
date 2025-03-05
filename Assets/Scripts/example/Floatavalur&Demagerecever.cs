using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagereciver : MonoBehaviour
{
   
    public FloatValue playerCurrentHealth;  // 用來儲存當前生命值
    public FloatValue heartContainers;  // 用來儲存最大生命值

    // 當玩家受到傷害時
    public void TakeDamage(float damage)
    {
        playerCurrentHealth.RuntimeValue -= damage;  // 減少生命值
        if (playerCurrentHealth.RuntimeValue <= 0)
        {
            playerCurrentHealth.RuntimeValue = 0;
            Die();  // 玩家死亡
        }
    }

    // 當玩家回血時
    public void Heal(float healAmount)
    {
        playerCurrentHealth.RuntimeValue += healAmount;  // 增加生命值
        if (playerCurrentHealth.RuntimeValue > heartContainers.RuntimeValue)  // 確保生命不會超過最大值
        {
            playerCurrentHealth.RuntimeValue = heartContainers.RuntimeValue;
        }
    }

    private void Die()
    {
        Debug.Log("玩家死亡！");
        // 處理死亡邏輯，例如播放動畫、摧毀物件等
    }
}