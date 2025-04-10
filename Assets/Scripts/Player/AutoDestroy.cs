using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        // 等待粒子系統的播放時間後銷毀物件
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
