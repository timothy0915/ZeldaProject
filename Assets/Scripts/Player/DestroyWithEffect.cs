using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithEffect : MonoBehaviour
{
    // ���w�ɤl�S�Ī�Prefab
    public GameObject deathEffect;

    // �I�s����k�i��P������
    public void DestroyObject()
    {
        // ��ҤƲɤl�S�Ħb��e���󪺦�m�M����
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // �P��������
        Destroy(gameObject);
    }
}
