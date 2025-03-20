using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireattack : MonoBehaviour
{
    public GameObject fireballPrefab; // 火球預製物
    public Transform firePoint;       // 發射火球的位置
    public float fireRate = 2f;       // 發射間隔（秒）
 

    private Transform player; // 玩家目標
    private void Start()
    {
        StartCoroutine(FireballRoutine()); // 開始循環發射火球
    }

    IEnumerator FireballRoutine()
    {
        while (true) // 無限循環，直到怪物死亡
        {
            yield return new WaitForSeconds(fireRate); // 等待指定時間
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
      
    }
}