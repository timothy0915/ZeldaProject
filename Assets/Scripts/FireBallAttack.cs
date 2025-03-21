using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAttack : MonoBehaviour
{
    [Header("火球設定")]
    public GameObject fireballPrefab; // 火球預製物
    public Transform firePoint;       // 發射火球的位置
    public float fireDelay = 1.8f;    // 發射延遲

    [Header("魔法攻擊設定")]
    public float attackCooldown = 2f; // 攻擊冷卻時間
    public float detectionRange = 10f; // 檢測玩家距離

    private float nextAttackTime = 0f; // 下次可攻擊時間

    private Animator animator;
    private Transform player; // 玩家目標
    void Start()
    {
        animator = GetComponent<Animator>(); // 取得動畫控制器

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        // 如果有偵測到玩家，並且在攻擊範圍內，就嘗試攻擊
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // 確認是否在偵測距離內 & 是否到達下一次攻擊時間
            if (distance <= detectionRange && Time.time >= nextAttackTime)
            {
                // 進行攻擊
                CastMagicAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void CastMagicAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("MagicAttack"); // 觸發魔法攻擊動畫
            Invoke(nameof(ShootFireball), fireDelay); // 讓火球稍後發射
        }
    }

    // **發射火球**
    private void ShootFireball()
    {
        if (fireballPrefab != null && firePoint != null && player != null)
        {
            // 計算 XZ 方向的朝向玩家方向
            Vector3 directionToPlayer = (player.position - firePoint.position);
            directionToPlayer.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);

            // 產生火球並朝向玩家方向（只在 XZ）
            Instantiate(fireballPrefab, firePoint.position, rotation);
        }
    }

}