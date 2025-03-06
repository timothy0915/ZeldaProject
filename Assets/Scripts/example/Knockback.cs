using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust = 10f;  // 擊退力度
    public float knockTime = 0.5f;  // 擊退時間
    public float damage = 10f;  // 傷害值
    public Collider hitCollider;  // 指定要開啟的碰撞箱
    private void Start()
    {
        hitCollider.enabled = false; // 一開始關閉碰撞箱
    }
    void Update()
    {
        // 按下 "左鍵" 開啟碰撞箱
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

                //  **根據攻擊來源決定擊退方向**
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * thrust, ForceMode.Impulse);

                StartCoroutine(KnockCo(enemyRb));
            }

            //  造成傷害
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
