using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SworOHS03rigibody : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // �������ܦ��B�ʾǼҦ��A�������z�v�T
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
