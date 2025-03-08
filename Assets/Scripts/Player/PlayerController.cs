using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // **角色控制元件**
    public CharacterController controller; // Unity 內建的 CharacterController 用於處理碰撞
    public Animator animator; // 角色動畫控制器

    [Header("Movement")] // **移動參數**
    public float speed = 3f; // 移動速度
    public float gravity = -9.81f; // 重力影響
    public float jumpHeight = 1f; // 跳躍高度

    [Header("Ground Check")] // **地面檢測**
    public Transform ground_check; // 用於檢測地面是否存在
    public float ground_distance = 0.5f; // 地面檢測範圍
    public LayerMask ground_mask; // 設置地面層，用於檢測地面

    [Header("Slope Handling")] // **坡度處理**
    public float slopeSpeedFactor = 0.5f; // 在坡面上的移動速度縮減係數
    public float maxSlopeAngle = 45f; // 最大坡度角度

    [Header("Knockback Settings")] // **擊退設定**
    public float knockbackDuration = 0.5f; // 擊退持續時間
    public float stunDuration = 0.5f; // 被擊退後的僵直時間

    // **私有變數**
    private Vector3 velocity; // 角色當前速度（包括重力影響）
    private bool isGrounded; // 是否接觸地面
    private Vector3 moveDirection; // 玩家移動方向
    private Vector3 knockbackDirection; // 擊退方向
    private float knockbackTimer = 0; // 擊退計時器
    private float stunTimer = 0; // 僵直計時器
    private bool isStunned = false; // 是否處於僵直狀態

    void Start()
    {
        animator = GetComponent<Animator>(); // 獲取角色動畫控制器
    }

    void Update()
    {
        // **檢測角色是否在地面上**
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, out _, ground_distance + 0.1f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f; // 避免角色在地面上時有微小的下落速度
        }

        // **處理擊退效果**
        if (knockbackTimer > 0)
        {
            controller.Move(knockbackDirection * Time.deltaTime); // 根據擊退方向移動
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isStunned = true;
                stunTimer = stunDuration; // 播放擊退結束後的僵直效果
            }
        }
        else if (isStunned)
        {
            // **處理僵直效果**
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false; // 僵直結束
            }
        }
        else
        {
            MovePlayer(); // 正常移動
        }
    }

    private void MovePlayer()
    {
        // **獲取玩家輸入**
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = new Vector3(x, 0, z);

        // **防止移動方向大於1（避免超過最大速度）**
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // **處理坡度影響速度**
        float slopeMultiplier = GetSlopeSpeedMultiplier();
        Vector3 finalMove = moveDirection * speed * slopeMultiplier;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection); // 角色面向移動方向
            animator.SetFloat("MoveSpeed", speed); // 播放移動動畫
            controller.Move(finalMove * Time.deltaTime); // 應用移動
        }
        else
        {
            animator.SetFloat("MoveSpeed", 0); // 停止移動動畫
        }

        // **攻擊 & 防禦**
        if (Input.GetKeyDown(KeyCode.Mouse0)) // 滑鼠左鍵攻擊
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) // 滑鼠右鍵防禦
        {
            animator.SetBool("defend", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("defend", false);
        }

        // **跳躍邏輯**
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // 計算跳躍速度
        }

        animator.SetBool("IsJump", !isGrounded); // 設定跳躍動畫狀態

        // **應用重力**
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // **應用擊退效果**
    public void ApplyKnockback(Vector3 direction, float force)
    {
        knockbackDirection = direction.normalized * force; // 設定擊退方向與力道
        knockbackTimer = knockbackDuration; // 開始擊退
        isStunned = false; // 取消僵直狀態
    }

    // **計算坡度影響的移動速度**
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up); // 計算坡度角度
            if (slopeAngle > maxSlopeAngle)
            {
                return slopeSpeedFactor; // 如果超過最大坡度角度，使用坡度減速係數
            }
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle); // 緩慢降低坡度上的速度
        }
        return 1f; // 默認返回正常速度
    }
}
