using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ĤH�����}���A�t�d����ĤH�������欰
public class EnemyAttack : MonoBehaviour
{
    // �����ѼƳ]�w
    public float attackRange = 1.5f;      // �������Ľd��]�ĤH�쪱�a���Z���^
    public float attackCooldown = 2f;     // �������j�ɶ��]�����N�o�ɶ��^
    public float knockbackForce = 5f;     // �����ɬI�[�b���a���W�����h�O��
    public float attackDamage = 10f;      // �����y�����ˮ`�q

    // �p���ܼ�
    private float attackTimer = 0f;       // �Ψӭp�ɧ����N�o���p�ɾ�
    private Animator animator;            // ����ʵe������
    private EnemyController enemyController; // �ѦҦP�@�Ӫ���W�� EnemyController�A�Ψ��ˬd�ĤH���ͩR���A
    public GameObject player;
    // Start() �禡�b�C���}�l�ɰ���@��
    private void Start()
    {
        // ���o��e����W�� Animator ����
        animator = GetComponent<Animator>();
        // ���o��e����W�� EnemyController ����A�H�K�����ˬd�ĤH�O�_���`
        enemyController = GetComponent<EnemyController>();
    }

    // Update() �C�@�V���|�Q�I�s�A�t�d��s�����N�o�p�ɻPĲ�o����
    private void Update()
    {
        // �p�G�ĤH�w�g���`�A�h����������޿�
        if (enemyController != null && enemyController.isDead)
        { 
            return; 
        }

        // �����N�o�p�ɡA�ϥ� deltaTime �ϭp�ɻP�V�v�L��
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            // ��N�o�p�ɧ�������է������a
            TryAttackPlayer();
            // ���m�N�o�p�ɾ�
            attackTimer = attackCooldown;
        }
    }

    // ���է������a����k
    private void TryAttackPlayer()
    {
        // �A���ˬd�ĤH�O�_���`�A�קK�b�P�@�V���o�ͧ����ɤw���`�����p
        if (enemyController != null && enemyController.isDead)
            return;

        // �ھ� Tag ��쪱�a����
      player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // �p�G�䤣�쪱�a�h���X�禡

        // �p��ĤH�M���a�������Z��
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // �p�G���a�b�����d�򤺫h�i�����
        if (distance <= attackRange)
        {
            // �p�G Animator �s�b�A�hĲ�o�����ʵe
            if (animator != null)
            {
                animator.SetTrigger("Attack");
                Invoke("Attaking", 0.5f);
            }

            
        }
    }
    void Attaking()
    {
        if (player == null) return; // �p�G�䤣�쪱�a�h���X�禡

        // �p��ĤH�M���a�������Z��
        float distance = Vector3.Distance(transform.position, player.transform.position);
        // �p�G���a�b�����d�򤺫h�i�����
        if (distance <= attackRange)
        {
            // �p�G Animator �s�b�A�hĲ�o����
            if (animator != null)
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
        
    }
}
