using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evilmsge : MonoBehaviour
{
    public GameObject fireballPrefab; // 火球預製體
    public Transform firePoint; // 火球生成點
    public float fireRate = 2f; // 發射間隔
    private float fireTimer;

    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            ShootFireball();
            fireTimer = 0;
        }
    }

    void ShootFireball()
    {
        // 生成火球，方向與怪物前方一致
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }
}