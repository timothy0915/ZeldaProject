using System.Collections;
using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    public Collider hitCollider;  // 指定要開啟的碰撞箱
    public float pushForce = 10f; // 推開的力量
    public float activeTime = 0.5f; // 碰撞箱開啟的時間

    private void Start()
    {
        hitCollider.enabled = false; // 一開始關閉碰撞箱
    }

    void Update()
    {
        // 按下 "右鍵" 開啟碰撞箱
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            hitCollider.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            hitCollider.enabled = false;
        }
    }

    void ActivateHitbox()
    {
        hitCollider.enabled = true;
       StartCoroutine(DisableHitboxAfterTime()); // 計時關閉
    }

    IEnumerator DisableHitboxAfterTime()
    {
        yield return new WaitForSeconds(activeTime);
        hitCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("enemy") || other.CompareTag("Rock"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // 計算推開方向
                Vector3 pushDirection = other.transform.position - transform.position;
                pushDirection.y = 0; // 避免物體飛到空中
                pushDirection.Normalize();

                // 加上推力
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
