using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust = 2f;  // ���h�O��
    public float knockTime = 0.5f;  // ���h�ɶ�
    public float damage = 1f;  // �ˮ`��

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
              Enemy enemyRb = other.GetComponent<Enemy>();
          Rigidbody enemyScript = other.GetComponent<Rigidbody>();

            if (enemyScript != null)
            {
                enemyScript.isKinematic = false;

                //  **�ھڧ����ӷ��M�w���h��V**
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemyScript.AddForce(knockbackDirection * thrust, ForceMode.Impulse);

                StartCoroutine(KnockCo(enemyScript));
            }

            //  �y���ˮ`
            if (enemyRb != null)
            {
                enemyRb.TakeDamage(damage);
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
