using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ˮ`�}���G���a�P����Ĳ�o���o�͸I���ɡA�缾�a�y���ˮ`�ìI�[���h�ĪG�C
/// ���}���q�`���[�b��������W�A�Q�� OnTriggerEnter �˴����a���i�J�ƥ�C
/// </summary>
public class TrapDamage : MonoBehaviour
{
    [Header("�����]�w")]
    public float damage = 10f;            // ���aĲ�I�����ɩҨ��쪺�ˮ`��
    public float knockbackForce = 5f;     // ���aĲ�o�����ɬI�[�����h�O��

    /// <summary>
    /// ���L�I�����i�J������ Trigger �ϰ�ɦ۰ʳQ�I�s�C
    /// </summary>
    /// <param name="other">�i�JĲ�o�����I����</param>
    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�I�����O�_�㦳 "Player" ���ҡA�T�{�i�J�̬����a
        if (other.CompareTag("Player"))
        {
            // ����������a�� PlayerController �}���A�T�O��H���T
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // �p��q�������߫��V���a��m���V�q
                Vector3 knockbackDir = other.transform.position - transform.position;
                // �N y ���q�]�� 0�A�T�O�Ȧb������V�p�����h��V
                knockbackDir.y = 0;
                // �Y�p��X���V�q�����s�A�h���W�ƥH��o����V
                if (knockbackDir != Vector3.zero)
                {
                    knockbackDir = knockbackDir.normalized;
                }
                // �I�s���a������� ApplyKnockback() ��k�A�I�[���h�ĪG
                player.ApplyKnockback(knockbackDir, knockbackForce);
                // �I�s���a������� TakeDamage() ��k�A�缾�a������q
                player.TakeDamage(damage);
            }
        }
    }
}