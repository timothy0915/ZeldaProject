using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 陷阱傷害腳本：當玩家與陷阱觸發器發生碰撞時，對玩家造成傷害並施加擊退效果。
/// 此腳本通常附加在陷阱物件上，利用 OnTriggerEnter 檢測玩家的進入事件。
/// </summary>
public class TrapDamage : MonoBehaviour
{
    [Header("陷阱設定")]
    public float damage = 10f;            // 當玩家觸碰陷阱時所受到的傷害值
    public float knockbackForce = 5f;     // 當玩家觸發陷阱時施加的擊退力度

    /// <summary>
    /// 當其他碰撞物進入陷阱的 Trigger 區域時自動被呼叫。
    /// </summary>
    /// <param name="other">進入觸發器的碰撞體</param>
    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰撞物是否具有 "Player" 標籤，確認進入者為玩家
        if (other.CompareTag("Player"))
        {
            // 嘗試獲取玩家的 PlayerController 腳本，確保對象正確
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // 計算從陷阱中心指向玩家位置的向量
                Vector3 knockbackDir = other.transform.position - transform.position;
                // 將 y 分量設為 0，確保僅在水平方向計算擊退方向
                knockbackDir.y = 0;
                // 若計算出的向量不為零，則正規化以獲得單位方向
                if (knockbackDir != Vector3.zero)
                {
                    knockbackDir = knockbackDir.normalized;
                }
                // 呼叫玩家控制器中的 ApplyKnockback() 方法，施加擊退效果
                player.ApplyKnockback(knockbackDir, knockbackForce);
                // 呼叫玩家控制器中的 TakeDamage() 方法，對玩家扣除血量
                player.TakeDamage(damage);
            }
        }
    }
}