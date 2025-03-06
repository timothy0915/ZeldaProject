using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public FloatValue maxHealth;  // �o�O Scriptable Object�A�s�̤j��q
    [SerializeField] float currentHealth;  // **���a�ܼơA��ڹB��ɪ���q**
    public Animator animator;
    public Collider Collider; //�Ǫ��I��

    private void Start()
    {
        currentHealth = maxHealth.initialValue;  // ��l�Ʀ�q
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} ����F {damage} �ˮ`�A�Ѿl��q: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} �Q���ѡI");
        // ���񦺤`�ʵe
        if (animator != null)
        {
            animator.SetTrigger("DEAD"); // ���ʵe���񦺤`�ʧ@
        }
    }
}
