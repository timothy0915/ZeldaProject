using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    // **敵人參數設定**
    public float speed = 3f; // 敵人的正常移動速度
    public float knockbackDuration = 0.5f; // 擊退持續時間
    public float stunDuration = 0.5f; // 被擊退後的僵直時間
    public Transform player; // 目標玩家

    // **內部變數**
    private CharacterController characterController; // 用於控制敵人的移動
    private Vector3 knockbackDirection; // 擊退的方向
    private float knockbackTimer = 0; // 擊退計時器
    private float stunTimer = 0; // 僵直計時器
    private bool isStunned = false; // 是否處於僵直狀態

    private void Start()
    {
        // 取得 CharacterController 組件
        characterController = GetComponent<CharacterController>();

        // **找到場景中的玩家對象（確保玩家有 "Player" 標籤）**
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            // **處理擊退效果**
            characterController.Move(knockbackDirection * Time.deltaTime); // 移動敵人
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                // 擊退結束後進入僵直狀態
                isStunned = true;
                stunTimer = stunDuration;
            }
        }
        else if (isStunned)
        {
            // **處理僵直效果**
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false; // 僵直結束，恢復行動
            }
        }
        else
        {
            // **正常情況下，敵人會朝玩家移動**
            MoveTowardsPlayer();
        }
    }

    // **施加擊退效果**
    public void ApplyKnockback(Vector3 direction, float force)
    {
        knockbackDirection = direction.normalized * force; // 設定擊退方向與力道
        knockbackTimer = knockbackDuration; // 設定擊退持續時間
        isStunned = false; // 確保不會因為擊退進入僵直狀態
    }

    // **敵人朝玩家移動**
    private void MoveTowardsPlayer()
    {
        if (player == null) return; // 確保玩家對象存在

        // 計算敵人到玩家的方向
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // 確保敵人不受 Y 軸影響（避免因地形高低改變移動方式）

        // **移動敵人朝玩家前進**
        characterController.Move(direction * speed * Time.deltaTime);

        // **讓敵人面向玩家**
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}
