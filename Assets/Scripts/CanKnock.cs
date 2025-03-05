using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public Animator animator;
public Collider hitCollider;  // ­n¼²ªºcollider
public class Canknock : MonoBehaviour

{
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isDead", false);
        animator.SetBool("getHit", false);
    }
    private void OnTriggerEnter(Collider other)
         Rigidbody rb = other.GetComponent<Rigidbody>();
     if (other.CompareTag("Sheld"))
            animator.SetBool("isDefend", true);
     if (other.CompareTag("Sheld")) animator.SetBool("isDefend", true);
}
// Update is called once per frame
void Update()
    {
        
    }
}
