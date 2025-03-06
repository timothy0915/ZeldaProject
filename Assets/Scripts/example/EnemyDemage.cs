using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemage : MonoBehaviour
{
    public float thrust = 2f;  // ���h�O��
    public float knockTime = 0.5f;  // ���h�ɶ�
    public float damage = 1f;  // �ˮ`��
    public Animator animator;
    public Collider Collider; //�Ǫ��I��

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  // �ܧ󬰧P�_���a
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            movement playerScript = other.GetComponent<movement>();  // �ܧ󬰪��a�}��

            if (playerRb != null)
            {
                playerRb.isKinematic = false;

                // **�ھڧ����ӷ��M�w���h��V**
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * thrust, ForceMode.Impulse);

                StartCoroutine(KnockCo(playerRb));
            }

            // �y���ˮ`
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
        }
    }

    private IEnumerator KnockCo(Rigidbody playerRb)
    {
        if (playerRb == null || playerRb.gameObject == null) yield break; // �T�O Rigidbody ���M�s�b

        yield return new WaitForSeconds(knockTime);

        if (playerRb != null && playerRb.gameObject != null) // �A���ˬd
        {
            playerRb.velocity = Vector3.zero;
            playerRb.isKinematic = true;
        }
    }
}
