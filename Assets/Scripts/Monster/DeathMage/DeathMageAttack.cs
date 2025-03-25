using UnityEngine;

public class DeathMageAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float knockbackForce = 5f;
    public float attackDamage = 3f;

    // �p�ɾ�
    private float attackTimer = 0f;
    private float specialAttackTimer = 0f; // �Ψӱ��� Attack�A�C 10 ��

    private bool isAttacking = false;

    private Animator animator;
    private DeathMageAI DeathMageAI;
    public GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        DeathMageAI = GetComponent<DeathMageAI>();
        specialAttackTimer = 10f; // �}�l�N�q 10 ��˼�
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
                // �C 10 ��Ĳ�o�@�� Attack3
                animator.SetTrigger("Attack3");
                specialAttackTimer = 10f; // ���]�S������p�ɾ�
            }
            else if (specialAttackTimer >= 0f)
            {
                // 50% �H���ϥ� Attack �� Attack2
                animator.SetTrigger("Attack");
            }

            //animator.SetTrigger(attackTrigger);
            isAttacking = true;

            Invoke(nameof(PerformMeleeAttack), 0.5f); // ����ˮ`
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

