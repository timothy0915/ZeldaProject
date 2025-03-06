using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemage : MonoBehaviour
{
    public float thrust = 2f;  // 擊退力度
    public float knockTime = 0.5f;  // 擊退時間
    public float damage = 1f;  // 傷害值
    public Animator animator;
    public Collider Collider; //怪物碰撞

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // 變更為判斷玩家
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            movement playerScript = other.GetComponent<movement>();  // 變更為玩家腳本

            if (playerRb != null)
            {
                playerRb.isKinematic = false;

                // **根據攻擊來源決定擊退方向**
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * thrust, ForceMode.Impulse);

                StartCoroutine(KnockCo(playerRb));
            }

            // 造成傷害
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody playerRb)
    {
        if (playerRb == null || playerRb.gameObject == null) yield break; // 確保 Rigidbody 仍然存在

        yield return new WaitForSeconds(knockTime);

        if (playerRb != null && playerRb.gameObject != null) // 再次檢查
        {
            playerRb.velocity = Vector3.zero;
            playerRb.isKinematic = true;
        }
    }
}
