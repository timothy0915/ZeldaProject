using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 玩家控制腳本，負責處理玩家的移動、攻擊、跳躍等操作
public class PlayerController : MonoBehaviour
{
    [Header("角色控制")]
    public CharacterController controller;  // 使用 Unity 的 CharacterController 處理碰撞和移動
    public Animator animator;               // 用於控制角色動畫

    [Header("移動設定")]
    public float speed = 3f;                // 移動速度
    public float gravity = -9.81f;          // 重力值
    public float jumpHeight = 1f;           // 跳躍高度

    [Header("地面檢測")]
    public Transform ground_check;          // 地面檢測點，用於判斷角色是否在地面上
    public float ground_distance = 0.5f;    // 檢測範圍的半徑
    public LayerMask ground_mask;           // 用於判斷哪些物體被認定為地面

    [Header("坡度處理")]
    public float slopeSpeedFactor = 0.5f;   // 當角色在坡道上移動時，速度的調整因子
    public float maxSlopeAngle = 45f;       // 最大坡度角度，超過此角度時速度會降低

    [Header("擊退設定")]
    public float knockbackDuration = 0.5f;  // 擊退持續的時間
    public float stunDuration = 0.5f;       // 僵直持續的時間

    [Header("攻擊設定")]
    public float attackRange = 2f;          // 攻擊檢測的射程（利用 Raycast）
    public float attackDamage = 20f;        // 攻擊造成的傷害
    public float attackKnockbackForce = 5f; // 攻擊時對敵人施加的擊退力度
    public float attackCooldown = 0.5f;     // 攻擊間隔時間
    private float attackTimer = 0f;         // 用於計算攻擊冷卻時間的計時器

    [Header("血量設定")]
    public float health = 100f;             // 玩家血量

    // 私有變數
    private Vector3 velocity;             // 用於計算角色受重力影響的移動速度
    private bool isGrounded;              // 判斷角色是否在地面上
    private Vector3 moveDirection;        // 玩家移動方向
    private Vector3 knockbackDirection;   // 擊退方向
    private float knockbackTimer = 0f;    // 擊退持續的計時器
    private float stunTimer = 0f;         // 僵直持續的計時器
    private bool isStunned = false;       // 是否處於僵直狀態
    public bool isDead = false;           // 玩家是否死亡

    // Start() 在遊戲開始時執行一次
    void Start()
    {
        // 取得 Animator 元件
        animator = GetComponent<Animator>();
    }

    // Update() 每一幀都會執行，處理玩家輸入與動作
    void Update()
    {
        // 如果玩家已死亡，則不處理後續輸入與行為
        if (isDead)
        {
            return; 
        }

        // 攻擊冷卻計時，每一幀減少冷卻計時器
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        // 地面檢測，利用 CharacterController 的 isGrounded 或使用 Raycast 檢測地面
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, ground_distance + 0.1f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            // 如果在地面上並且下落速度為負，則將下落速度設定為輕微的負值以保持接地狀態
            velocity.y = -2f;
        }

        // 處理擊退和僵直狀態
        if (knockbackTimer > 0)
        {
            // 在擊退狀態中，利用 CharacterController 移動
            controller.Move(knockbackDirection * Time.deltaTime);
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
            // 處於僵直狀態，倒數計時
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                // 僵直結束
                isStunned = false;
            }
        }
        else
        {
            // 玩家正常控制移動
            MovePlayer();
            // 攻擊判定：當玩家按下滑鼠左鍵並且攻擊冷卻時間結束時
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackTimer <= 0)
            {
                // 觸發攻擊動畫
                animator.SetTrigger("attack");
                // 執行攻擊的射線檢測
                AttackRaycast();
                // 重置攻擊冷卻計時器
                attackTimer = attackCooldown;
            }
        }
    }

    // 處理玩家移動的邏輯
    private void MovePlayer()
    {
        // 取得水平（x 軸）和垂直（z 軸）的輸入值
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // 建立一個向量表示移動方向（僅限 x 與 z 軸）
        moveDirection = new Vector3(x, 0, z);
        // 如果輸入向量大於 1，則進行正規化，以保持移動速度一致
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // 計算在坡道上移動時的速度調整係數
        float slopeMultiplier = GetSlopeSpeedMultiplier();
        // 計算最終移動向量：基於輸入方向、速度和坡道調整因子
        Vector3 finalMove = moveDirection * speed * slopeMultiplier;

        if (moveDirection != Vector3.zero)
        {
            // 如果有移動輸入，則讓角色面向移動方向
            transform.rotation = Quaternion.LookRotation(moveDirection);
            // 設定移動動畫參數，這裡設為速度值
            animator.SetFloat("MoveSpeed", speed);
            // 利用 CharacterController 移動角色
            controller.Move(finalMove * Time.deltaTime);
        }
        else
        {
            // 沒有移動輸入時，將移動動畫參數設為0，表示靜止
            animator.SetFloat("MoveSpeed", 0);
        }

        // 處理跳躍邏輯：當按下跳躍按鍵且角色在地面上時
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // 計算跳躍的初始速度，利用跳躍高度與重力公式
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // 設定動畫中是否在空中（跳躍狀態），根據是否在地面上
        animator.SetBool("IsJump", !isGrounded);
        // 累加重力影響，使角色逐漸加速下墜
        velocity.y += gravity * Time.deltaTime;
        // 利用 CharacterController 移動角色，包括垂直方向的重力效果
        controller.Move(velocity * Time.deltaTime);
    }

    // 利用射線檢測進行攻擊判定
    private void AttackRaycast()
    {
        // 建立一條射線：從角色上方（模擬胸部高度）向前發射
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;
        // 如果射線在 attackRange 內碰撞到物體
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // 檢查碰撞物是否標記為 "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // 取得被碰撞物上的 EnemyController 腳本
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    // 使敵人受到傷害
                    enemy.TakeDamage(attackDamage);
                    // 計算施加在敵人身上的擊退方向（從玩家指向敵人）
                    Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                    // 使敵人受到擊退效果
                    enemy.ApplyKnockback(knockbackDir, attackKnockbackForce);
                }
            }
        }
    }

    // 當玩家受到外部擊退時呼叫此方法
    public void ApplyKnockback(Vector3 direction, float force)
    {
        // 設定擊退方向為正規化後乘上力度
        knockbackDirection = direction.normalized * force;
        // 設定擊退持續時間
        knockbackTimer = knockbackDuration;
        // 在受到擊退時，取消僵直狀態
        isStunned = false;
    }

    // 當玩家受到攻擊時呼叫，處理傷害和動畫
    public void TakeDamage(float damage)
    {
        // 撥放被攻擊動畫，請確保 Animator 中設有 "GetHit" 的 Trigger
        animator.SetTrigger("GetHit");

        // 扣除血量
        health -= damage;
        // 如果血量小於等於 0，則執行死亡程序
        if (health <= 0f)
        {
            Die();
        }
    }

    // 處理玩家死亡邏輯
    private void Die()
    {
        // 在 Console 輸出死亡訊息
        Debug.Log("Player died.");
        // 觸發死亡動畫，Animator 中應設有 "Die" 的 Trigger
        animator.SetTrigger("Die");

        // 設定死亡旗標，避免後續動作
        isDead = true;

        // 停用角色控制器，防止角色在死亡後繼續移動
        controller.enabled = false;

        // 改變標籤，讓敵人不再將其視作目標
        gameObject.tag = "Untagged";

        // 此處可以添加遊戲結束或重生的其他邏輯
    }

    // 計算坡道對移動速度的影響，回傳速度乘數
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        // 從角色位置向下發射射線檢測地面
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            // 計算地面法線與垂直方向之間的夾角，這就是坡度角
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle)
            {
                // 如果坡度大於最大允許角度，返回較低的速度乘數
                return slopeSpeedFactor;
            }
            // 否則根據坡度角進行線性插值，返回介於 1 和 slopeSpeedFactor 之間的乘數
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle);
        }
        // 若沒有檢測到地面，返回默認速度乘數 1
        return 1f;
    }
}