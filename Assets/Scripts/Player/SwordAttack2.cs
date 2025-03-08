using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack2 : MonoBehaviour
{
    public float knockbackForce = 5f; // 擊退的力度
    public float damage = 20f; // 攻擊造成的傷害（目前未應用傷害系統）

    // **當劍的碰撞體接觸到其他對象時觸發**
    private void OnTriggerEnter(Collider other)
    {
        // **檢測碰撞的對象是否是敵人**
        if (other.CompareTag("Enemy"))
        {
            // **計算擊退方向**
            Vector3 knockbackDirection = other.transform.position - transform.position; // 由劍的位置到敵人的方向
            knockbackDirection.y = 0; // 防止敵人被擊飛到空中
            knockbackDirection.Normalize(); // 使擊退方向標準化

            // **獲取敵人的 EnemyController 腳本**
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // **對敵人應用擊退效果**
                enemy.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}
