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
    // �w�q TakeDamage ��k�A����^�ǳѾl�ͩR��
    public float TakeDamage(float damage)
    {
        currentHealth -= damage;
        return currentHealth; // �^�ǳѾl�ͩR��
    }
}
