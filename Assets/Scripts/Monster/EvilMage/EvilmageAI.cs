using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilmageAI : MonoBehaviour, IDamageable   //IDamageable使各項AI統一整合

{
    [Header("移動參數")]
    public float speed = 0f;              // 移動速度
    public float detectionRange = 10f;      // 偵測玩家的距離
    public float knockbackDuration = 0.5f;  // 擊退持續的時間
    public float stunDuration = 0.5f;       // 僵直持續的時間

    [Header("血量設定")]
    public float health = 3f;             // 敵人初始血量
    public Transform player;              // 玩家物件的 Transform
    public Animator animator;             // 控制動畫的 Animator

    // 私有變數
    private CharacterController characterController; // 用於移動角色的 CharacterController 元件
    private Vector3 knockbackDirection;  // 記錄擊退的方向
    private float knockbackTimer = 0f;   // 記錄擊退效果持續時間的計時器
    private float stunTimer = 0f;        // 記錄僵直效果持續時間的計時器
    private bool isStunned = false;      // 是否處於僵直狀態
    public bool isDead = false;          // 是否已經死亡
    public Transform MyTransform => transform;

    // Start() 在遊戲開始時執行一次
    private void Start()
    {
        // 取得角色控制器，負責移動與碰撞
        characterController = GetComponent<CharacterController>();

        // 若 animator 尚未被指定，嘗試從當前物件取得 Animator 元件
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        // 利用 Tag 尋找玩家物件，並取得其 Transform
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Update() 每一幀執行一次，負責處理敵人的行為
    private void Update()
    {
        // 如果敵人已死亡，則不處理任何行為
        if (isDead)
        {
            return;
        }

        // 檢查是否處於擊退或僵直狀態
        if (knockbackTimer > 0)
        {
            // 在擊退狀態下，利用角色控制器移動
            characterController.Move(knockbackDirection * Time.deltaTime);
            // 減少擊退計時器
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                // 當擊退效果結束後，進入僵直狀態
                isStunned = true;
                stunTimer = stunDuration;
            }
            // 播放站立動畫，不進行移動動畫
            animator.SetBool("IsMoving", false);
        }
        else if (isStunned)
        {
            // 處於僵直狀態時，倒數計時
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                // 僵直時間結束，取消僵直狀態
                isStunned = false;
            }
            // 播放站立動畫
            animator.SetBool("IsMoving", false);
        }
        else
        {
            // 若不在擊退或僵直狀態，則進行正常移動向玩家靠近
            MoveTowardsPlayer();
        }
    }

    // 方法：讓敵人移動向玩家靠近
    private void MoveTowardsPlayer()
    {
        // 如果找不到玩家則直接返回
        if (player == null) return;

        // 檢查玩家是否死亡（假設玩家身上有 PlayerController）
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.isDead)
        {
            // 若玩家已死亡，停止移動與攻擊
            animator.SetBool("IsMoving", false);
            return;
        }

        // 計算敵人與玩家之間的距離
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // 如果玩家在偵測範圍內則追趕玩家
        if (distanceToPlayer <= detectionRange)
        {
            // 計算從敵人指向玩家的方向
            Vector3 direction = (player.position - transform.position);
            // 僅考慮水平方向
            direction.y = 0;
            if (direction.magnitude > 0)
            {
                // 使敵人面向玩家
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                // 播放移動動畫
                animator.SetBool("IsMoving", true);
            }
            else
            {
                // 如果方向向量為零，則停止移動動畫
                animator.SetBool("IsMoving", false);
            }
            // 使用 SimpleMove 方法移動敵人，SimpleMove 自動考慮重力效果
            characterController.SimpleMove(direction.normalized * speed);
        }
        else
        {
            // 如果玩家超出偵測範圍，停止移動動畫
            animator.SetBool("IsMoving", false);
        }
    }

    // 方法：處理受到攻擊時減少血量的行為
    public void TakeDamage(float damage)
    {
        // 如果已經死亡則忽略傷害
        if (isDead)
        {
            return;
        }
        // 扣除血量
        health -= damage;
        // 觸發受傷動畫
        animator.CrossFade("GetHit", 0f, 0);
        // 如果血量小於等於 0，則執行死亡程序
        if (health <= 0f)
        {
            Die();
        }

    }

    // 方法：當敵人受到攻擊時施加擊退效果
    public void ApplyKnockback(Vector3 direction, float force)
    {
        // 如果已死亡則不處理
        if (isDead) return;

        // 設定擊退方向為正規化後的方向乘上力度
        knockbackDirection = direction.normalized * force;
        // 設定擊退計時器為預設持續時間
        knockbackTimer = knockbackDuration;
        // 在擊退期間取消僵直狀態
        isStunned = false;
    }

    // 方法：處理敵人死亡的邏輯
    private void Die()
    {
        // 防止重複執行死亡邏輯
        if (isDead) return;
        isDead = true;

        // 觸發死亡動畫，在 Animator 中應該有 "Die" 的 Trigger
        animator.CrossFade("Die", 0f, 0);

        // 停用角色控制器，避免死亡後仍進行碰撞或移動
        characterController.enabled = false;

        // 如果存在攻擊腳本，則停用攻擊行為
        EvilmageAI EvilmageAI = GetComponent<EvilmageAI>();
        if (EvilmageAI != null)
        {
            EvilmageAI.enabled = false;
        }

        // 啟動協程，等待死亡動畫播放完畢後銷毀物件
        StartCoroutine(DeathRoutine());
    }

    // 協程：等待一段時間後銷毀敵人物件
    private IEnumerator DeathRoutine()
    {
        // 等待 2 秒，可以根據死亡動畫長度進行調整
        yield return new WaitForSeconds(2f);

        // 等待結束後銷毀該物件
        Destroy(gameObject);
    }
}