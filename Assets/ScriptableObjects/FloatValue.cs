using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    public float initialValue;
    [HideInInspector]
    public float RuntimeValue;
    public void OnAfterDeserialize()
    {
        RuntimeValue = initialValue;
    }
    public void OnBeforeSerialize() { }
    // 定義 TakeDamage 方法，讓其回傳剩餘生命值
    public float TakeDamage(float damage)
    {
        currentHealth -= damage;
        return currentHealth; // 回傳剩餘生命值
    }
}
