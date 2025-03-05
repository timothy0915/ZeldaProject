using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SworOHS03rigibody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 讓剛體變成運動學模式，不受物理影響
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
