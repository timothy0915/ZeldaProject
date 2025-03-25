using UnityEngine;

public class DeathMageAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float knockbackForce = 5f;
    public float attackDamage = 3f;

    // 計時器
    private float attackTimer = 0f;
    private float specialAttackTimer = 0f; // 用來控制 Attack，每 10 秒

    private bool isAttacking = false;

    private Animator animator;
    private DeathMageAI DeathMageAI;
    public GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        DeathMageAI = GetComponent<DeathMageAI>();
        specialAttackTimer = 10f; // 開始就從 10 秒倒數
    }

    void Update()
    {
        if (DeathMageAI != null && DeathMageAI.isDead) return;

        attackTimer -= Time.deltaTime;
        specialAttackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && !isAttacking)
        {
            TryAttackPlayer();
        }
    }

    void TryAttackPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            //string attackTrigger;

            if (specialAttackTimer <= 0f)
            {
                // 每 10 秒觸發一次 Attack3
                animator.SetTrigger("Attack3");
                specialAttackTimer = 10f; // 重設特殊攻擊計時器
            }
            else if (specialAttackTimer >= 0f)
            {
                // 50% 隨機使用 Attack 或 Attack2
                animator.SetTrigger("Attack");
            }

            //animator.SetTrigger(attackTrigger);
            isAttacking = true;

            Invoke(nameof(PerformMeleeAttack), 0.5f); // 延遲傷害
            Invoke(nameof(ResetAttackState), attackCooldown);
        }
    }

    void ResetAttackState()
    {
        isAttacking = false;
        attackTimer = attackCooldown;
    }

    void PerformMeleeAttack()
    {
        if (player == null) return;

        Vector3 knockbackDirection = player.transform.position - transform.position;
        knockbackDirection.y = 0;
        knockbackDirection.Normalize();

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ApplyKnockback(knockbackDirection, knockbackForce);
            playerController.TakeDamage(attackDamage);
        }
    }
}

