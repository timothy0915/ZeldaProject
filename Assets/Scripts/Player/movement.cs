using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class movement : MonoBehaviour
{
    public FloatValue maxHealth;  // 這是 Scriptable Object，存最大血量
    public float currentHealth;  // **本地變數，實際運行時的血量**

    public CharacterController controller; // 負責控制角色移動
    public Animator animator; // 控制角色動畫

    [Header("Movement")]
    public float speed = 3f; // 角色的基本移動速度
    public float gravity = -9.81f; // 重力值，負值代表向下
    public float jumpHeight = 1f; // 跳躍高度

    [Header("Ground Check")]
    public Transform ground_check; // 角色腳下的檢測點，用來判斷是否著地
    public float ground_distance = 0.5f; // 地面檢測範圍
    public LayerMask ground_mask; // 設定哪種層級是地面（用來檢測角色是否在地面上）

    [Header("Slope Handling")]
    public float slopeSpeedFactor = 0.5f; // 當坡度最大時，速度縮減的比例
    public float maxSlopeAngle = 45f; // 最大坡度角度，超過此值則移動速度大幅下降

    Vector3 velocity; // 角色的垂直速度（用於跳躍和重力）
    bool isGrounded; // 記錄角色是否在地面上
    private Vector3 moveDirection; // 移動方向

    void Start()

    {
        animator = GetComponent<Animator>();// 取得角色的 Animator 組件
        currentHealth = maxHealth.initialValue;
    }

    void Update()
    {
        // **檢測角色是否在地面上**
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, out _, ground_distance + 0.45f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f; // 讓角色緊貼地面，避免不必要的浮動
        }

        // **取得玩家輸入**
        float x = Input.GetAxis("Horizontal"); // 水平移動
        float z = Input.GetAxis("Vertical");   // 垂直移動

        // 設定移動方向（不包含 Y 軸，避免影響重力）
        moveDirection = new Vector3(x, 0, z);

        // 確保移動方向的長度不超過 1，防止對角線移動時速度加快
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // **坡度速度調整**
        float slopeMultiplier = GetSlopeSpeedMultiplier(); // 根據坡度計算速度縮減比例
        Vector3 finalMove = moveDirection * speed * slopeMultiplier; // 計算最終移動速度

        // **移動角色**
        if (moveDirection != Vector3.zero) // 如果有輸入移動鍵
        {
            transform.rotation = Quaternion.LookRotation(moveDirection); // 角色朝向移動方向
            animator.SetFloat("MoveSpeed", speed); // 播放跑步動畫
            controller.Move(finalMove * Time.deltaTime); // 角色移動
        }
        else
        {
            animator.SetFloat("MoveSpeed", 0); // 停止跑步動畫
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))//按下左鍵攻擊
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))//按下右鍵防禦
        {
            animator.SetBool("defend", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))//放開右鍵復原
        {
            animator.SetBool("defend", false);
        }

        // **跳躍邏輯**
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // 計算跳躍速度
        }

        // **動畫控制**
        if (isGrounded)
        {
            animator.SetBool("IsJump", false); // 在地面時，關閉跳躍動畫
        }
        else
        {
            animator.SetBool("IsJump", true); // 在空中時，開啟跳躍動畫
        }

        // **應用重力**
        velocity.y += gravity * Time.deltaTime; // 讓角色的 Y 軸速度受到重力影響
        controller.Move(velocity * Time.deltaTime); // 讓角色受到重力影響而下落
    }

    // **計算坡度影響速度的函式**
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        // 向腳下發射一條射線，檢測坡度
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up); // 計算坡度角度

            // 如果坡度超過最大設定角度，速度降至最低
            if (slopeAngle > maxSlopeAngle)
            {
                return slopeSpeedFactor;
            }

            // 根據坡度比例縮減速度（坡度越大，速度越慢）
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle);
        }
        return 1f;
    }// 預設回傳 1（代表不影響速度）
    
    //碰撞傷害
    private void OnTriggerEnter(Collider other)
    {
        DamageReciever target = other.GetComponent<DamageReciever>();
        if (target != null)
        {
            // 傳遞傷害並取得剩餘生命值
        target.TakeDamage(1);//碰撞傷害值

        }
    }
     public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} 受到了 {damage} 傷害，剩餘血量: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log($"{gameObject.name} 被擊敗！");
        Destroy(gameObject);  // 銷毀敵人
    }
}
