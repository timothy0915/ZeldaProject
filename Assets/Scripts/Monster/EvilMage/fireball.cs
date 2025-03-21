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
        float distanceThisFrame = speed * Time.deltaTime;

        // 在前方做 Raycast 偵測是否會撞到東西（牆或其他）
        if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, distanceThisFrame))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(2); // 傷害
                }
            }

            // 不論撞到什麼都銷毀火球（牆或玩家）
            Destroy(gameObject);
            return;
        }

        // 沒撞到就照原本方向移動
        transform.position += moveDirection * distanceThisFrame;

        // 計時銷毀
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}

