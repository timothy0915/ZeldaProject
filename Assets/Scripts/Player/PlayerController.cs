using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制腳本：負責處理玩家的移動、攻擊、跳躍、受到攻擊以及死亡等狀態。
/// 利用 Unity 的 CharacterController 來實現碰撞檢測與物理移動，並使用 Animator 控制動畫播放。
/// </summary>
public class PlayerController : MonoBehaviour
{
    
    [Header("角色控制")]
    public CharacterController controller;  // 利用 Unity 內建的 CharacterController 處理角色的碰撞和移動
    public Animator animator;               // 用於控制角色動畫的 Animator 元件
    public MusicPlayer musicPlayer;


    [Header("移動設定")]
    public float speed = 3f;                // 角色的基本移動速度
    public float gravity = -9.81f;          // 模擬重力的數值（負值表示向下）
    public float jumpHeight = 1f;           // 跳躍能達到的高度
   

   
    [Header("地面檢測")]
    public Transform ground_check;          // 地面檢測點，通常放在角色腳部附近
    public float ground_distance = 0.5f;      // 以此半徑進行地面碰撞檢查
    public LayerMask ground_mask;           // 指定哪些 Layer 被認定為地面（例如 Terrain、Platform 等）
    

    
    [Header("坡度處理")]
    public float slopeSpeedFactor = 0.5f;   // 當角色在坡道上時，移動速度的倍率（越小代表坡度大時速度降低得越明顯）
    public float maxSlopeAngle = 45f;       // 允許角色移動的最大坡度角度，超過此角度後移動速度會被進一步調整
  

    
    [Header("擊退設定")]
    public float knockbackDuration = 0.5f;  // 當角色受到擊退時，持續移動的時間
    public float stunDuration = 0.5f;       // 擊退結束後，角色處於僵直狀態的持續時間（無法操作）
   

    
    [Header("攻擊設定")]
    public float attackRange = 2f;          // 攻擊時使用 Raycast 檢測的射程距離
    public float attackDamage = 20f;        // 攻擊時造成敵人的傷害值
    public float attackKnockbackForce = 5f; // 攻擊時對敵人施加的擊退力量
    public float attackCooldown = 0.5f;     // 攻擊後需要等待的冷卻時間
    private float attackTimer = 0f;         // 用來計時攻擊冷卻的計時器
   

   
    // 透過連擊變數控制攻擊招式連續輸入的狀態
    private int attackCombo = 0;            // 目前的連擊狀態（例如 1 表示第一招，2 表示第二招）
    public float comboResetTime = 1.0f;       // 連擊輸入間隔，若超過這個時間則重置連擊狀態
    private float comboTimer = 0f;            // 計時連擊間隔的倒計時器
   

   
    [Header("血量設定")]
    public float health = 100f;             // 玩家初始的血量值
   
   
    private Vector3 velocity;             // 用來計算重力、跳躍與其他外力影響下的速度
    private bool isGrounded;              // 是否接觸地面的旗標
    private Vector3 moveDirection;        // 玩家移動方向的向量
    private Vector3 knockbackDirection;   // 擊退時的方向向量
    private float knockbackTimer = 0f;    // 擊退狀態的持續倒計時
    private float stunTimer = 0f;         // 僵直狀態的倒計時
    private bool isStunned = false;       // 是否正處於僵直狀態（無法操作）
    public bool isDead = false;           // 玩家是否已經死亡
   

    // Start() 在遊戲開始時執行一次，通常用來初始化必要元件
    void Start()
    {
        // 取得該 GameObject 上的 Animator 元件，用以控制動畫
        animator = GetComponent<Animator>();
    }

    // Update() 每一幀呼叫一次，用於處理玩家輸入及狀態更新
    void Update()
    {
        // 若玩家已死亡，不再處理任何輸入或狀態更新
        if (isDead)
        {
            return;
        }

        // 攻擊冷卻：若攻擊計時器尚未歸零，則持續減少倒數
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // 地面檢測：利用 CharacterController 的 isGrounded 或 Raycast 來確認角色是否在地面上
        // 此處同時檢查兩種方式以提高穩定性
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, ground_distance + 0.1f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            // 當角色落地時，將垂直速度設為輕微負值以保證持續接地，避免因速度過大造成穿透
            velocity.y = -2f;
        }

        // 處理擊退與僵直狀態
        if (knockbackTimer > 0)
        {
            // 正在受到擊退：根據 knockbackDirection 移動角色
            controller.Move(knockbackDirection * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                // 擊退結束後，進入短暫的僵直狀態
                isStunned = true;
                stunTimer = stunDuration;
            }
        }
        else if (isStunned)
        {
            // 處於僵直狀態：倒數計時直至結束
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                // 僵直狀態結束，恢復玩家控制
                isStunned = false;
            }
        }
        else
        {
            // 正常操作狀態下，處理玩家移動和攻擊輸入
            MovePlayer();

            // 當玩家按下滑鼠左鍵且攻擊冷卻計時器歸零時進行攻擊
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackTimer <= 0)
            {
                // 判斷是否在有效連擊輸入時間內
                if (comboTimer > 0)
                {
                    // 若連擊次數還未達上限，則增加連擊數
                    if (attackCombo < 2)
                        attackCombo++;
                    else
                        attackCombo = 1;  // 若已達第二招，則重置為第一招（依需求可調整連擊上限）
                }
                else
                {
                    // 若連擊間隔超時，從第一招開始
                    attackCombo = 1;
                }

                // 根據當前連擊狀態觸發對應的攻擊動畫
                if (attackCombo == 1)
                    animator.SetTrigger("attack1");
                else if (attackCombo == 2)
                    animator.SetTrigger("attack2");


                // 執行攻擊檢測（利用 Raycast 判定前方是否有敵人命中）
                AttackRaycast();
                // 重置攻擊冷卻計時器
                attackTimer = attackCooldown;
                // 重置連擊計時器，等待下一次連擊輸入
                comboTimer = comboResetTime;
            }

            // 更新連擊倒計時器，若倒計時結束則重置連擊狀態
            if (comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
            }
            else
            {
                attackCombo = 0;
            }
        }

        // 處理跳躍：檢查是否按下跳躍按鈕且角色位於地面上
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // 根據跳躍高度及重力計算需要的初始垂直速度
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // 更新動畫狀態：當角色不在地面上時，將 "IsJump" 參數設為 true
        animator.SetBool("IsJump", !isGrounded);
        // 依照重力持續影響垂直速度
        velocity.y += gravity * Time.deltaTime;
        // 使用 CharacterController 移動角色，同時考慮重力效果
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// MovePlayer() 處理玩家的基本移動操作，包括取得輸入、計算方向、考慮坡度影響、以及更新動畫。
    /// </summary>
    private void MovePlayer()
    {
        // 從玩家輸入取得水平（x 軸）與垂直（z 軸）的數值
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // 組合成移動方向向量，忽略 y 軸（垂直方向）
        moveDirection = new Vector3(x, 0, z);
        // 如果向量長度大於 1（例如同時按下兩個方向鍵），則進行正規化以保持一致速度
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // 根據目前地面的坡度調整移動速度倍率
        float slopeMultiplier = GetSlopeSpeedMultiplier();
        // 計算最終移動向量：基於移動方向、設定速度與坡度倍率
        Vector3 finalMove = moveDirection * speed * slopeMultiplier;

        if (moveDirection != Vector3.zero)
        {
            // 當有移動輸入時，旋轉角色面向移動方向
            transform.rotation = Quaternion.LookRotation(moveDirection);
            // 設定動畫參數，根據速度播放對應的移動動畫
            animator.SetFloat("MoveSpeed", speed);
            // 執行移動操作
            controller.Move(finalMove * Time.deltaTime);
        }
        else
        {
            // 無輸入時，設定動畫參數為 0，播放待機狀態動畫
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    /// <summary>
    /// AttackRaycast() 利用射線檢測判斷攻擊是否命中敵人，
    /// 若命中則對敵人造成傷害並施加擊退效果。
    /// </summary>
    private void AttackRaycast()
    {
        // 射線從角色位置向上偏移後發出，方向與角色正前方一致
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;
        // 在 attackRange 距離內檢測射線碰撞
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // 如果碰撞物標籤為 "Enemy"，進行攻擊處理
            if (hit.collider.CompareTag("Enemy"))
            {
                // 嘗試獲取敵人的控制腳本
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    // 使敵人受到攻擊傷害
                    enemy.TakeDamage(attackDamage);
                    // 計算從玩家到敵人的方向（正規化後作為擊退方向）
                    Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                    // 對敵人施加擊退效果，推離玩家
                    enemy.ApplyKnockback(knockbackDir, attackKnockbackForce);
                    musicPlayer.s_hit();
                    Debug.Log("中");
                }
            }
            else
            {
                musicPlayer.s_swing();
                Debug.Log("揮");
            }
        }
        else
        {
            musicPlayer.s_swing();
            Debug.Log("揮");
        }
    }

    /// <summary>
    /// ApplyKnockback() 當玩家受到外部擊退（例如被敵人攻擊或碰到陷阱）時呼叫，
    /// 根據指定方向與力度執行擊退效果，並重置相關狀態。
    /// </summary>
    /// <param name="direction">擊退的方向向量</param>
    /// <param name="force">擊退力度</param>
    public void ApplyKnockback(Vector3 direction, float force)
    {
        // 設定擊退方向，並乘上力度以獲得最終移動速度
        knockbackDirection = direction.normalized * force;
        // 啟動擊退計時器
        knockbackTimer = knockbackDuration;
        // 清除僵直狀態，確保此次外力效果立即發生
        isStunned = false;
    }

    /// <summary>
    /// TakeDamage() 當玩家受到攻擊時呼叫，
    /// 播放受擊動畫並扣除相應血量，若血量低於零則呼叫死亡邏輯。
    /// </summary>
    /// <param name="damage">攻擊造成的傷害數值</param>
    public void TakeDamage(float damage)
    {
        // 播放受擊動畫，顯示玩家被攻擊的效果
        animator.SetTrigger("GetHit");
        // 扣除玩家血量
        health -= damage;
        // 當血量低於等於 0 時，觸發死亡程序
        if (health <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Die() 處理玩家死亡時的行為，包括播放死亡動畫、禁用角色控制器，
    /// 並將 GameObject 標籤移除以避免後續不必要的交互。
    /// </summary>
    private void Die()
    {
        Debug.Log("Player died.");
        // 播放死亡動畫
        animator.SetTrigger("Die");
        // 標記玩家為死亡狀態，防止後續操作
        isDead = true;
        // 禁用 CharacterController 以停止角色移動與碰撞檢測
        controller.enabled = false;
        // 移除玩家標籤，避免其他物件繼續識別為玩家
        gameObject.tag = "Untagged";
    }

    /// <summary>
    /// GetSlopeSpeedMultiplier() 根據角色下方地面的坡度計算一個速度倍率，
    /// 坡度越陡，倍率越低，使角色在上坡或下坡時移動速度有所調整。
    /// </summary>
    /// <returns>移動速度的倍率</returns>
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        // 從角色位置向下發射射線，檢測地面狀態
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            // 計算碰撞面法線與垂直方向的夾角，即坡度角
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            // 若坡度角超過最大允許角度，返回較低的速度倍率
            if (slopeAngle > maxSlopeAngle)
            {
                return slopeSpeedFactor;
            }
            // 根據坡度角與最大角度的比例，線性內插獲得一個平滑的速度倍率
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle);
        }
        // 若無法檢測到地面，則返回正常移動速度倍率
        return 1f;
    }
}