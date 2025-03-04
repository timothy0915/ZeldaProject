using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterbehavorMashroom : MonoBehaviour
//蘑菇怪物動作

{
    public Animator animator;
    public float HP;
    public float MaxHP;
    public Collider hitCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()//主要動作
    {
        MaxHP = 5;
        if (hitCollider.enabled == true)

        {
            HP = MaxHP -= 1;
        }
        if (HP <= 0) 
        {
            animator.SetBool("DEAD", true);
        }
    }
}
