using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        // 单采╰参冀丁綪反ン
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
