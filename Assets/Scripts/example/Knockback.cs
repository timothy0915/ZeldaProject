using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust = 10f;  // ���h�O��
    public float knockTime = 0.5f;  // ���h�ɶ�
    public float damage = 10f;  // �ˮ`��
    public Collider hitCollider;  // ���w�n�}�Ҫ��I���c
    private void Start()
    {
        hitCollider.enabled = false; // �@�}�l�����I���c
    }
    void Update()
    {
        // ���U "����" �}�ҸI���c
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = other.GetComponent<Rigidbody>();
            Enemy enemyScript = other.GetComponent<Enemy>();

            if (enemyRb != null)
            {
                enemyRb.isKinematic = false;

                //  **�ھڧ����ӷ��M�w���h��V**
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * thrust, ForceMode.Impulse);

                StartCoroutine(KnockCo(enemyRb));
            }

            //  �y���ˮ`
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody enemyRb)
    {
        if (enemyRb != null)
        {
            yield return new WaitForSeconds(knockTime);
            enemyRb.velocity = Vector3.zero;
            enemyRb.isKinematic = true;
        }
    }
}
