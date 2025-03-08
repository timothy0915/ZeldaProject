using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // 攻擊範圍（敵人能攻擊到玩家的最遠距離）
    public float attackCooldown = 2f; // 攻擊冷卻時間（兩次攻擊之間的間隔時間）
    public float knockbackForce = 5f; // 擊退力道（攻擊時給玩家的擊退力度）

    private float attackTimer = 0; // 記錄攻擊冷卻時間的計時器

    private void Update()
    {
        // **減少攻擊計時器，確保敵人攻擊間隔**
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            TryAttackPlayer(); // 嘗試攻擊玩家
            attackTimer = attackCooldown; // 重置攻擊計時器
        }
    }

    // **嘗試對玩家進行攻擊**
    private void TryAttackPlayer()
    {
        // **找到場景中的玩家對象（確保玩家有 "Player" 標籤）**
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // 確保玩家對象存在

        // **計算敵人與玩家之間的距離**
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackRange) // 如果玩家在攻擊範圍內
        {
            // **計算擊退方向（從敵人位置指向玩家）**
            Vector3 knockbackDirection = player.transform.position - transform.position;
            knockbackDirection.y = 0; // 確保擊退不影響 Y 軸，避免玩家被擊飛
            knockbackDirection.Normalize(); // 使擊退方向標準化

            // **獲取玩家的 PlayerController 腳本**
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // **對玩家應用擊退效果**
                playerController.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}
