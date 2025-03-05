using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagereciver : MonoBehaviour
{
   
    public FloatValue playerCurrentHealth;  // �Ψ��x�s��e�ͩR��
    public FloatValue heartContainers;  // �Ψ��x�s�̤j�ͩR��

    // ���a����ˮ`��
    public void TakeDamage(float damage)
    {
        playerCurrentHealth.RuntimeValue -= damage;  // ��֥ͩR��
        if (playerCurrentHealth.RuntimeValue <= 0)
        {
            playerCurrentHealth.RuntimeValue = 0;
            Die();  // ���a���`
        }
    }

    // ���a�^���
    public void Heal(float healAmount)
    {
        playerCurrentHealth.RuntimeValue += healAmount;  // �W�[�ͩR��
        if (playerCurrentHealth.RuntimeValue > heartContainers.RuntimeValue)  // �T�O�ͩR���|�W�L�̤j��
        {
            playerCurrentHealth.RuntimeValue = heartContainers.RuntimeValue;
        }
    }

    private void Die()
    {
        Debug.Log("���a���`�I");
        // �B�z���`�޿�A�Ҧp����ʵe�B�R������
    }
}