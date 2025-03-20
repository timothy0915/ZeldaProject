using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireattack : MonoBehaviour
{
    public GameObject fireballPrefab; // 火球預製物
    public Transform firePoint;       // 發射火球的位置
    public float fireRate = 2f;       // 發射間隔（秒）
    public float fireDelay = 0.5f;    // 發射延遲

    private Animator animator;

    private Transform player; // 玩家目標
    private void Start()
    {
        animator = GetComponent<Animator>(); // 取得動畫控制器
    }

    public void CastMagicAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("MagicAttack"); // 觸發魔法攻擊動畫
            Invoke(nameof(ShootFireball), fireDelay); // 讓火球稍後發射
        }
    }

    // **發射火球**
    private void ShootFireball()
    {
        if (fireballPrefab != null && firePoint != null)
        {
            Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
        }
    }
}