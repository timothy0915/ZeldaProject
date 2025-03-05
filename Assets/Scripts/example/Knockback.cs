using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust = 2f;  // 擊退力度
    public float knockTime = 0.5f;  // 擊退時間
    public float damage = 1f;  // 傷害值

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
              Enemy enemyRb = other.GetComponent<Enemy>();
          Rigidbody enemyScript = other.GetComponent<Rigidbody>();

            if (enemyScript != null)
            {
                enemyScript.isKinematic = false;

                //  **根據攻擊來源決定擊退方向**
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemyScript.AddForce(knockbackDirection * thrust, ForceMode.Impulse);

                StartCoroutine(KnockCo(enemyScript));
            }

            //  造成傷害
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
