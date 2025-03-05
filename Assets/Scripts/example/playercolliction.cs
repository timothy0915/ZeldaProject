using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercolliction : MonoBehaviour

{
    public float damage = 1; // 傷害值

    private void OnTriggerEnter(Collider other)
    {
        FloatValue target = other.GetComponent<FloatValue>();
        if (target != null)
        {
            // 傳遞傷害並取得剩餘生命值
            float remainingHealth = target.TakeDamage(damage);

            // 顯示剩餘生命值
            Debug.Log("目標剩餘生命：" + remainingHealth);
        }
    }
}