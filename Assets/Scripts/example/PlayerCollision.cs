using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float knockbackForce = 5f; // �Q���h���O�q
    public float knockbackDuration = 0.2f; // �Q���h������ɶ�
    private bool isKnockedBack = false;
    private Vector3 knockbackDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // ���o Rigidbody
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isKnockedBack)
        {
            // �p�����h��V (�q�ĤH��m���V���a)
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
            rb.velocity = knockbackDirection * knockbackForce; // �z�L Rigidbody �I�[���h�O
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector3.zero; // �������h
        isKnockedBack = false;
    }
}
