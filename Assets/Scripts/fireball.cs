using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // 火球速度
    public float lifetime = 1f; // 火球存在時間
    private float timer = 0f;

    private void Update()
    {
        // 火球沿著自己的 forward 方向移動
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

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
        else if (other.CompareTag("Collider"))
        {
            Destroy(gameObject); // 碰到牆壁就銷毀
        }
    }
}