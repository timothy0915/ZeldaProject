using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider hitCollider;  // 指定要開啟的碰撞箱
    public float pushForce = 10f; // 推開的力量
    

    private void Start()
    {
        hitCollider.enabled = false; // 一開始關閉碰撞箱
    }

    void Update()
    {
        // 按下 "右鍵" 開啟碰撞箱
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
        // 只影響標籤為 "Enemy" 
        if (other.CompareTag("Enemy"))
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
