using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("VoidSword") || other.CompareTag("Sheld"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log(rb);
                if (other.CompareTag("VoidSword"))
                {
                    Destroy(this.gameObject);
                }
            }
        }

    }
}
