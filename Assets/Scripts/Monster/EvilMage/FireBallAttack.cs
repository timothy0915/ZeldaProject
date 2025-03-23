using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAttack : MonoBehaviour
{
    [Header("���y�]�w")]
    public GameObject fireballPrefab; // ���y�w�s��
    public Transform firePoint;       // �o�g���y����m
    public float fireDelay = 1.8f;    // �o�g����

    [Header("�]�k�����]�w")]
    public float attackCooldown = 2f; // �����N�o�ɶ�
    public float detectionRange = 10f; // �˴����a�Z��

    private float nextAttackTime = 0f; // �U���i�����ɶ�

    private Animator animator;
    private Transform player; // ���a�ؼ�
    void Start()
    {
        animator = GetComponent<Animator>(); // ���o�ʵe���

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        // �p�G�������쪱�a�A�åB�b�����d�򤺡A�N���է���
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // �T�{�O�_�b�����Z���� & �O�_��F�U�@�������ɶ�
            if (distance <= detectionRange && Time.time >= nextAttackTime)
            {
                // �i�����
                CastMagicAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    public void CastMagicAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("MagicAttack"); // Ĳ�o�]�k�����ʵe
            Invoke(nameof(ShootFireball), fireDelay); // �����y�y��o�g
        }
    }

    // **�o�g���y**
    private void ShootFireball()
    {
        if (fireballPrefab != null && firePoint != null && player != null)
        {
            // �p�� XZ ��V���¦V���a��V
            Vector3 directionToPlayer = (player.position - firePoint.position);
            directionToPlayer.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);

            // ���ͤ��y�ô¦V���a��V�]�u�b XZ�^
            Instantiate(fireballPrefab, firePoint.position, rotation);
        }
    }

}