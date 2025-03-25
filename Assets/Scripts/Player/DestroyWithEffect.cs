using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithEffect : MonoBehaviour
{
    // ﹚采疭Prefab
    public GameObject deathEffect;

    // ㊣よ猭秈︽綪反ン
    public void DestroyObject()
    {
        // 龟ㄒて采疭讽玡ン竚㎝臂锣
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // 綪反ン
        Destroy(gameObject);
    }
}
