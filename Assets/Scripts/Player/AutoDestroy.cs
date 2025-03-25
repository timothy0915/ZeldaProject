using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        // ���ݲɤl�t�Ϊ�����ɶ���P������
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
