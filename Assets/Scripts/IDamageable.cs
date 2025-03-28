using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
    void ApplyKnockback(Vector3 direction, float force);
    Transform MyTransform { get; }
}