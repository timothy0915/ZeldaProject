using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵人攻擊腳本，負責控制敵人的攻擊行為
public class EnemyAttack : MonoBehaviour
{
    // 攻擊參數設定
    public float attackRange = 1.5f;      // 攻擊有效範圍（敵人到玩家的距離）
    public float attackCooldown = 2f;     // 攻擊間隔時間（攻擊冷卻時間）
    public float knockbackForce = 5f;     // 攻擊時施加在玩家身上的擊退力度
    public float attackDamage = 10f;      // 攻擊造成的傷害量

    // 私有變數
    private float attackTimer = 0f;       // 用來計時攻擊冷卻的計時器
    private Animator animator;            // 控制動畫的元件
    private EnemyController enemyController; // 參考同一個物件上的 EnemyController，用來檢查敵人的生命狀態
    public GameObject player;
    // Start() 函式在遊戲開始時執行一次
    private void Start()
    {
        // 取得當前物件上的 Animator 元件
        animator = GetComponent<Animator>();
        // 取得當前物件上的 EnemyController 元件，以便後續檢查敵人是否死亡
        enemyController = GetComponent<EnemyController>();
    }

    // Update() 每一幀都會被呼叫，負責更新攻擊冷卻計時與觸發攻擊
    private void Update()
    {
        // 如果敵人已經死亡，則不執行攻擊邏輯
        if (enemyController != null && enemyController.isDead)
        { 
            return; 
        }

        // 攻擊冷卻計時，使用 deltaTime 使計時與幀率無關
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            // 當冷卻計時完畢後嘗試攻擊玩家
            TryAttackPlayer();
            // 重置冷卻計時器
            attackTimer = attackCooldown;
        }
    }

    // 嘗試攻擊玩家的方法
    private void TryAttackPlayer()
    {
        // 再次檢查敵人是否死亡，避免在同一幀中發生攻擊時已死亡的情況
        if (enemyController != null && enemyController.isDead)
            return;

        // 根據 Tag 找到玩家物件
      player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // 如果找不到玩家則跳出函式

        // 計算敵人和玩家之間的距離
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // 如果玩家在攻擊範圍內則進行攻擊
        if (distance <= attackRange)
        {
            // 如果 Animator 存在，則觸發攻擊動畫
            if (animator != null)
            {
                animator.SetTrigger("Attack");
                Invoke("Attaking", 0.5f);
            }

            
        }
    }
    void Attaking()
    {
        if (player == null) return; // 如果找不到玩家則跳出函式

        // 計算敵人和玩家之間的距離
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // 如果玩家在攻擊範圍內則進行攻擊
        if (distance <= attackRange)
        {
            // 如果 Animator 存在，則觸發攻擊
            if (animator != null)
            {
                // 計算擊退方向：從敵人指向玩家的方向
                Vector3 knockbackDirection = player.transform.position - transform.position;
                // 保持水平方向，不考慮 y 軸
                knockbackDirection.y = 0;
                // 將方向向量正規化，使其長度為 1
                knockbackDirection.Normalize();

                // 取得玩家的 PlayerController 來對玩家施加傷害和擊退效果
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    // 呼叫玩家的 ApplyKnockback 方法施加擊退效果
                    playerController.ApplyKnockback(knockbackDirection, knockbackForce);
                    // 呼叫玩家的 TakeDamage 方法扣除生命值
                    playerController.TakeDamage(attackDamage);
                }
            }
        }
        
    }
}
