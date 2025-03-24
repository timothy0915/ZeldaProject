using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMageAttack : MonoBehaviour
{
    [Header("�����ѼƳ]�w")]
    public float attackRange = 1.5f;               // ��ԧ����Z��
    public float fireballAttackRange = 10f;        // ���y�����Z���W��
    public float attackCooldown = 2f;              // �����N�o�ɶ�
    public float meleeDamage = 2f;
    public float meleeKnockback = 5f;
    public float fireballDelay = 1.8f;
    public GameObject fireballPrefab;
    public Transform firePoint;

    private Animator animator;
    private EvilmageAI evilmageAI;
    private Transform player;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        evilmageAI = GetComponent<EvilmageAI>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (evilmageAI != null && evilmageAI.isDead) return;
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (attackTimer <= 0f && !isAttacking)
        {
            if (distance <= attackRange)
            {
                DoMeleeAttack();
            }
            else if (distance > attackRange && distance <= fireballAttackRange)
            {
                DoFireballAttack();
            }
        }
    }

    void DoMeleeAttack()
    {
        isAttacking = true;

        // �T�O���|�A�o���y
        CancelInvoke(nameof(ShootFireball));

        animator.ResetTrigger("MagicAttack");
        animator.SetTrigger("Attack");
        Invoke(nameof(ApplyMeleeDamage), 0.5f);
        Invoke(nameof(ResetAttack), attackCooldown);
    }


    void ApplyMeleeDamage()
    {
        if (player == null) return;

        Vector3 knockbackDir = (player.position - transform.position);
        knockbackDir.y = 0;
        knockbackDir.Normalize();

        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ApplyKnockback(knockbackDir, meleeKnockback);
            pc.TakeDamage(meleeDamage);
        }
    }

    void DoFireballAttack()
    {
        isAttacking = true;
        animator.ResetTrigger("Attack");
        animator.SetTrigger("MagicAttack");
        Invoke(nameof(ShootFireball), fireballDelay);
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void ShootFireball()
    {
        if (fireballPrefab == null || firePoint == null || player == null) return;

        Vector3 direction = player.position - firePoint.position;
        direction.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(direction);

        Instantiate(fireballPrefab, firePoint.position, rotation);
    }

    void ResetAttack()
    {
        isAttacking = false;
        attackTimer = attackCooldown;
    }
}