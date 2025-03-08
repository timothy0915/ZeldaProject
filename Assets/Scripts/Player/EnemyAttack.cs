using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f; // �����d��]�ĤH������쪱�a���̻��Z���^
    public float attackCooldown = 2f; // �����N�o�ɶ��]�⦸�������������j�ɶ��^
    public float knockbackForce = 5f; // ���h�O�D�]�����ɵ����a�����h�O�ס^

    private float attackTimer = 0; // �O�������N�o�ɶ����p�ɾ�

    private void Update()
    {
        // **��֧����p�ɾ��A�T�O�ĤH�������j**
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            TryAttackPlayer(); // ���է������a
            attackTimer = attackCooldown; // ���m�����p�ɾ�
        }
    }

    // **���չ缾�a�i�����**
    private void TryAttackPlayer()
    {
        // **�������������a��H�]�T�O���a�� "Player" ���ҡ^**
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // �T�O���a��H�s�b

        // **�p��ĤH�P���a�������Z��**
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackRange) // �p�G���a�b�����d��
        {
            // **�p�����h��V�]�q�ĤH��m���V���a�^**
            Vector3 knockbackDirection = player.transform.position - transform.position;
            knockbackDirection.y = 0; // �T�O���h���v�T Y �b�A�קK���a�Q����
            knockbackDirection.Normalize(); // �����h��V�зǤ�

            // **������a�� PlayerController �}��**
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // **�缾�a�������h�ĪG**
                playerController.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}
