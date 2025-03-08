using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack2 : MonoBehaviour
{
    public float knockbackForce = 5f; // ���h���O��
    public float damage = 20f; // �����y�����ˮ`�]�ثe�����ζˮ`�t�Ρ^

    // **��C���I���鱵Ĳ���L��H��Ĳ�o**
    private void OnTriggerEnter(Collider other)
    {
        // **�˴��I������H�O�_�O�ĤH**
        if (other.CompareTag("Enemy"))
        {
            // **�p�����h��V**
            Vector3 knockbackDirection = other.transform.position - transform.position; // �ѼC����m��ĤH����V
            knockbackDirection.y = 0; // ����ĤH�Q������Ť�
            knockbackDirection.Normalize(); // �����h��V�зǤ�

            // **����ĤH�� EnemyController �}��**
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                // **��ĤH�������h�ĪG**
                enemy.ApplyKnockback(knockbackDirection, knockbackForce);
            }
        }
    }
}
