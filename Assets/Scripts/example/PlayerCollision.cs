using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float knockbackForce = 5f; // 被擊退的力量
    public float knockbackDuration = 0.2f; // 被擊退的持續時間
    private bool isKnockedBack = false;
    private Vector3 knockbackDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // 取得 Rigidbody
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isKnockedBack)
        {
            // 計算擊退方向 (從敵人位置指向玩家)
            knockbackDirection = (transform.position - collision.transform.position).normalized;
            isKnockedBack = true;
            StartCoroutine(KnockbackCoroutine());
        }
    }

    private System.Collections.IEnumerator KnockbackCoroutine()
    {
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            rb.velocity = knockbackDirection * knockbackForce; // 透過 Rigidbody 施加擊退力
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector3.zero; // 停止擊退
        isKnockedBack = false;
    }
}
