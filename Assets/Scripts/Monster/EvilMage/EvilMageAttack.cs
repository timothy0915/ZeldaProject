using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMageAttack : MonoBehaviour

{
    // 攻擊參數設定
    public float attackRange = 1.5f;      // 攻擊有效範圍（敵人到玩家的距離）
    public float attackCooldown = 2f;     // 攻擊間隔時間（攻擊冷卻時間）
    public float knockbackForce = 5f;     // 攻擊時施加在玩家身上的擊退力度
    public float attackDamage = 2f;      // 攻擊造成的傷害量
    public float magicDamage = 2f;              // 魔法傷害
    public float magicKnockbackForce = 0f;      // 魔法擊退力
    public float fireballattackrange = 10f;   //火球攻擊距離
    

    // 私有變數
    private float attackTimer = 0f;       // 用來計時攻擊冷卻的計時器
    private bool isAttacking = false;

    private Animator animator;            // 控制動畫的元件
    private EvilmageAI EvilmageAI; // 參考同一個物件上的 EvilmageAI，用來檢查敵人的生命狀態
    public GameObject player;

    // Start() 函式在遊戲開始時執行一次
    void Start()
    {
        // 取得當前物件上的 Animator 元件
        animator = GetComponent<Animator>();
        // 取得當前物件上的 EvilmageAI 元件，以便後續檢查敵人是否死亡
        EvilmageAI = GetComponent<EvilmageAI>();
    }

    // Update() 每一幀都會被呼叫，負責更新攻擊冷卻計時與觸發攻擊
    void Update()
    {
        // 如果敵人已經死亡，則不執行攻擊邏輯
        if (EvilmageAI != null && EvilmageAI.isDead)
        {
            return;
        }

        // 攻擊冷卻計時，使用 deltaTime 使計時與幀率無關
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f && !isAttacking)
        {
            // 當冷卻計時完畢後嘗試攻擊玩家
            TryAttackPlayer();

        }
    }

    // 嘗試攻擊玩家的方法
    private void TryAttackPlayer()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        //判斷是否為近戰
        if (distance <= attackRange)
        {
            // 播放近戰動畫並延遲執行傷害
            animator.ResetTrigger("MagicAttack"); // 確保不會同時觸發
            animator.SetTrigger("Attack");
            isAttacking = true;
            Invoke(nameof(AttackingMelee), 0.5f);
            Invoke(nameof(ResetAttackState), attackCooldown);

        }
        else if (distance > attackRange && distance <= fireballattackrange)
        {
         
            {
                // 播放遠程魔法動畫
                animator.ResetTrigger("Attack");
                animator.SetTrigger("MagicAttack");
                isAttacking = true;
                Invoke(nameof(AttackingMagic), 0.5f);
                Invoke(nameof(ResetAttackState), attackCooldown);
            }
        }
    }
    void ResetAttackState()
    {
        isAttacking = false;
        attackTimer = attackCooldown; // 共用冷卻時間
    }
    void AttackingMagic()
    {
        Vector3 knockbackDirection = player.transform.position - transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ApplyKnockback(knockbackDirection, magicKnockbackForce);
            playerController.TakeDamage(magicDamage);
        }
    }
    void AttackingMelee()
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