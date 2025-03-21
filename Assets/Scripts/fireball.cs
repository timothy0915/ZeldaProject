using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // 火球速度
    public float lifetime = 1f; // 火球存在時間
    private float timer = 0f; 
    private Vector3 moveDirection; // 記錄火球的移動方向

    private void Start()
    {
        // 取得火球發射的方向，確保只在 XZ 平面移動
        moveDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    }

    private void Update()
    {
        // 火球移動
        transform.position += moveDirection * speed * Time.deltaTime;

        // 計時銷毀火球
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerHealth = other.GetComponent<PlayerController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2); // 扣 (2) 點血
            }
            Destroy(gameObject); // 火球消失
        }
        else if (!other.isTrigger) // 非 Trigger 碰撞如牆壁
        {
            Destroy(gameObject);
        }
    }
}