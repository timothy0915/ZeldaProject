using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider hitCollider;  // ���w�n�}�Ҫ��I���c

    private void Start()
    {
        hitCollider.enabled = false; // �@�}�l�����I���c
    }

    void Update()
    {
        // ���U "�k��" �}�ҸI���c
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            hitCollider.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            hitCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �u�v�T���Ҭ� "Enemy" �� "Rock" ������
        if (other.CompareTag("enemy") || other.CompareTag("Grass"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
