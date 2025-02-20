using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorCtrl : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Move(float Speed)
    {
        anim.SetFloat("MoveSpeed", Speed);
    }
    public void Attack(bool attack, int hit) 
    {
        anim.SetBool("attacking", attack);
        if (attack)
        {
            anim.SetInteger("attack", hit);
        }
    }
    public void Defend(bool defend)
    {
        anim.SetBool("defending", defend);
    }
    public void Focus(bool focus)
    {
        anim.SetBool("focusing", focus);
    }
    public void Spin(bool spin)
    {
        anim.SetBool("spinning", spin);
    }












}
