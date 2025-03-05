using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercolliction : MonoBehaviour

{
    public float damage = 1; // �ˮ`��

    private void OnTriggerEnter(Collider other)
    {
        FloatValue target = other.GetComponent<FloatValue>();
        if (target != null)
        {
            // �ǻ��ˮ`�è��o�Ѿl�ͩR��
            float remainingHealth = target.TakeDamage(damage);

            // ��ܳѾl�ͩR��
            Debug.Log("�ؼгѾl�ͩR�G" + remainingHealth);
        }
    }
}