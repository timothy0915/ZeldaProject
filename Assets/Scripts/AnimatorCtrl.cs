using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl : MonoBehaviour
{
     Animator anim;
    int hit= 0;
    float timer_f= 0f;
    int timer_i= 0;
    float hitTime = 0f ;
    float focusTime = 0f;
    int spinTime = 0;
    float pastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer_f += Time.deltaTime;
        if (timer_f-1 > timer_i)
        {
            timer_i = (int)timer_f;
            Debug.Log(timer_i);
            if (spinTime>=1)
            {
                spinTime -= 1;
                if(spinTime<=0)
                {
                    anim.SetBool("spinning", false);
                }
            }
        }
        if ( Input.GetKeyDown(KeyCode.Mouse0))//按下左鍵(單次觸發
        {
            pastTime = timer_i - hitTime;
     /*
            if (k < 0.1f)
            {
                return;
            }
     */     
            anim.SetBool("attacking", true);
            if (hit >= 3 || pastTime >= 3f)
            {
                hit = 1;
            }
            else
            {
                hit += 1;
            }
            anim.SetInteger("attack", hit);
            Debug.Log("在"+ pastTime + "秒後的第" + hit + "擊");
            hitTime = focusTime = timer_f;
            anim.SetBool("focusing", true);
        }
        if (Input.GetKey(KeyCode.Mouse0))//按住左鍵(持續觸發
        {
            pastTime = timer_i - focusTime +0.7f ;
            if (pastTime >= 1)
            {
                anim.SetBool("attacking", false);
            }
        }

            if (Input.GetKeyUp(KeyCode.Mouse0))//放開左鍵
        {
            anim.SetBool("attacking", false);
            pastTime = timer_i - focusTime;
            if (pastTime >= 2)
            {
                anim.SetBool("spinning", true);
                spinTime = 2;
            }
            anim.SetBool("focusing", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))//按下右鍵
        {
            anim.SetBool("defending", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))//放開左鍵
        {
            anim.SetBool("defending", false);
        }

    }
}
