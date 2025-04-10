using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithEffect : MonoBehaviour
{
    // 指定粒子特效的Prefab
    public GameObject deathEffect;

    // 呼叫此方法進行銷毀物件
    public void DestroyObject()
    {
        // 實例化粒子特效在當前物件的位置和旋轉
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // 銷毀此物件
        Destroy(gameObject);
    }
}
