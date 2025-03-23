using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMageAttack : MonoBehaviour

{
    // �����ѼƳ]�w
    public float attackRange = 1.5f;      // �������Ľd��]�ĤH�쪱�a���Z���^
    public float attackCooldown = 2f;     // �������j�ɶ��]�����N�o�ɶ��^
    public float knockbackForce = 5f;     // �����ɬI�[�b���a���W�����h�O��
    public float attackDamage = 2f;      // �����y�����ˮ`�q
    public float magicDamage = 2f;              // �]�k�ˮ`
    public float magicKnockbackForce = 0f;      // �]�k���h�O
    public float fireballattackrange = 10f;   //���y�����Z��
    

    // �p���ܼ�
    private float attackTimer = 0f;       // �Ψӭp�ɧ����N�o���p�ɾ�
    private bool isAttacking = false;

    private Animator animator;            // ����ʵe������
    private EvilmageAI EvilmageAI; // �ѦҦP�@�Ӫ���W�� EvilmageAI�A�Ψ��ˬd�ĤH���ͩR���A
    public GameObject player;

    // Start() �禡�b�C���}�l�ɰ���@��
    void Start()
    {
        // ���o��e����W�� Animator ����
        animator = GetComponent<Animator>();
        // ���o��e����W�� EvilmageAI ����A�H�K�����ˬd�ĤH�O�_���`
        EvilmageAI = GetComponent<EvilmageAI>();
    }

    // Update() �C�@�V���|�Q�I�s�A�t�d��s�����N�o�p�ɻPĲ�o����
    void Update()
    {
        // �p�G�ĤH�w�g���`�A�h����������޿�
        if (EvilmageAI != null && EvilmageAI.isDead)
        {
            return;
        }

        // �����N�o�p�ɡA�ϥ� deltaTime �ϭp�ɻP�V�v�L��
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f && !isAttacking)
        {
            // ��N�o�p�ɧ�������է������a
            TryAttackPlayer();

        }
    }

    // ���է������a����k
    private void TryAttackPlayer()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        //�P�_�O�_�����
        if (distance <= attackRange)
        {
            // �����԰ʵe�é������ˮ`
            animator.ResetTrigger("MagicAttack"); // �T�O���|�P��Ĳ�o
            animator.SetTrigger("Attack");
            isAttacking = true;
            Invoke(nameof(AttackingMelee), 0.5f);
            Invoke(nameof(ResetAttackState), attackCooldown);

        }
        else if (distance > attackRange && distance <= fireballattackrange)
        {
         
            {
                // ���񻷵{�]�k�ʵe
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
        attackTimer = attackCooldown; // �@�ΧN�o�ɶ�
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
        // �p�����h��V�G�q�ĤH���V���a����V
        Vector3 knockbackDirection = player.transform.position - transform.position;
        // �O��������V�A���Ҽ{ y �b
        knockbackDirection.y = 0;
        // �N��V�V�q���W�ơA�Ϩ���׬� 1
        knockbackDirection.Normalize();

        // ���o���a�� PlayerController �ӹ缾�a�I�[�ˮ`�M���h�ĪG
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // �I�s���a�� ApplyKnockback ��k�I�[���h�ĪG
            playerController.ApplyKnockback(knockbackDirection, knockbackForce);
            // �I�s���a�� TakeDamage ��k�����ͩR��
            playerController.TakeDamage(attackDamage);
        }
    }
}