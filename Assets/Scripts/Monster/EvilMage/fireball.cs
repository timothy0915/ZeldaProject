using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // ���y�t��
    public float lifetime = 1f; // ���y�s�b�ɶ�
    private float timer = 0f; 
    private Vector3 moveDirection; // �O�����y�����ʤ�V

    private void Start()
    {
        // ���o���y�o�g����V�A�T�O�u�b XZ ��������
        moveDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    }

    private void Update()

    {
        float distanceThisFrame = speed * Time.deltaTime;

        // �b�e�谵 Raycast �����O�_�|����F��]��Ψ�L�^
        if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, distanceThisFrame))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(2); // �ˮ`
                }
            }

            // ���׼��줰�򳣾P�����y�]��Ϊ��a�^
            Destroy(gameObject);
            return;
        }

        // �S����N�ӭ쥻��V����
        transform.position += moveDirection * distanceThisFrame;

        // �p�ɾP��
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}

