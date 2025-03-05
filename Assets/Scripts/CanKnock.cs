using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanKnock : MonoBehaviour
{    
    public Animator animator;
    public Collider hitCollider;  // ­n¼²ªºcollider

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isDead", false);
        animator.SetBool("getHit", false);
    }
    private void OnTriggerEnter(Collider other);
         Rigidbody rb = other.GetComponent<Rigidbody>();
     if (other.CompareTag("Sheld"))
            animator.SetBool("isDefend", true);
     if (other.CompareTag("Sheld")) animator.SetBool("isDefend", true);
}

// Update is called once per frame
static void Update()
    {
        
    }
}
