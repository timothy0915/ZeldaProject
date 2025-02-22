using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider hitCollider;  // 指定要開啟的碰撞箱

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
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("enemy") || other.CompareTag("Grass"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
