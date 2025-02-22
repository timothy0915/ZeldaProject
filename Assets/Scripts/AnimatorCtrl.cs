using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorCtrl : MonoBehaviour
{
    Animator anim;

    // Invoke( "anim.SetBool("attacking", false)" ,2);這條可以延後觸發函式，之後把指令全用函式存就可以用了
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
    public void Ground(bool ground)
    {
        anim.SetBool("grounded", ground);
    }
    public void Jump(bool jump)
    {
        anim.SetBool("jumping", jump);
    }
}
