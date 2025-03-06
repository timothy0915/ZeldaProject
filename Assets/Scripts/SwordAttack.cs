using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider hitCollider;  // ���w�n�}�Ҫ��I���c
    public float pushForce = 10f; // ���}���O�q
    

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
        // �u�v�T���Ҭ� "Enemy" 
        if (other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                // �p����}��V
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.y = 0; // �קK���魸��Ť�
                pushDirection.Normalize();

                // �[�W���O
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
           
        }
    }
}
