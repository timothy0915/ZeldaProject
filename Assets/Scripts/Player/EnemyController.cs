using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    // **�ĤH�ѼƳ]�w**
    public float speed = 3f; // �ĤH�����`���ʳt��
    public float knockbackDuration = 0.5f; // ���h����ɶ�
    public float stunDuration = 0.5f; // �Q���h�᪺�����ɶ�
    public Transform player; // �ؼЪ��a

    // **�����ܼ�**
    private CharacterController characterController; // �Ω󱱨�ĤH������
    private Vector3 knockbackDirection; // ���h����V
    private float knockbackTimer = 0; // ���h�p�ɾ�
    private float stunTimer = 0; // �����p�ɾ�
    private bool isStunned = false; // �O�_�B��������A

    private void Start()
    {
        // ���o CharacterController �ե�
        characterController = GetComponent<CharacterController>();

        // **�������������a��H�]�T�O���a�� "Player" ���ҡ^**
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            // **�B�z���h�ĪG**
            characterController.Move(knockbackDirection * Time.deltaTime); // ���ʼĤH
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                // ���h������i�J�������A
                isStunned = true;
                stunTimer = stunDuration;
            }
        }
        else if (isStunned)
        {
            // **�B�z�����ĪG**
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false; // ���������A��_���
            }
        }
        else
        {
            // **���`���p�U�A�ĤH�|�ª��a����**
            MoveTowardsPlayer();
        }
    }

    // **�I�[���h�ĪG**
    public void ApplyKnockback(Vector3 direction, float force)
    {
        knockbackDirection = direction.normalized * force; // �]�w���h��V�P�O�D
        knockbackTimer = knockbackDuration; // �]�w���h����ɶ�
        isStunned = false; // �T�O���|�]�����h�i�J�������A
    }

    // **�ĤH�ª��a����**
    private void MoveTowardsPlayer()
    {
        if (player == null) return; // �T�O���a��H�s�b

        // �p��ĤH�쪱�a����V
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // �T�O�ĤH���� Y �b�v�T�]�קK�]�a�ΰ��C���ܲ��ʤ覡�^

        // **���ʼĤH�ª��a�e�i**
        characterController.Move(direction * speed * Time.deltaTime);

        // **���ĤH���V���a**
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
}
