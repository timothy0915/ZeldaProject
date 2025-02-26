using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHpConter : MonoBehaviour
{
    public int HP;
    public int TimeI;
    public Animator animator;
    public Collider hitCollider;  // 指定要開啟的碰撞箱
    // Start is called before the first frame update
    void Start()
    {
        HP = 3;
        animator.SetBool("isDead", false);
        animator.SetBool("getHit", false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("Sword") || other.CompareTag("Sheld")|| other.CompareTag("VoidSword"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log(rb);
                if (other.CompareTag("Sword")|| other.CompareTag("VoidSword"))
                {
                    UnityEngine.Debug.Log("Ouch");
                    if (Timer.GetTimer.timer_i - TimeI >= 1)
                    {
                        animator.SetBool("getHit", true);
                        HP -= 1;
                        TimeI = Timer.GetTimer.timer_i;
                        if (HP <= 0)
                        {
                            HP = 0;
                            animator.SetBool("isDead", true);
                        }
                    }
                }
                if (other.CompareTag("Sheld")) animator.SetBool("isDefend", true);
            }
            Invoke("animOver", 1);
        }
        
    }
    void animOver()
    {
        animator.SetBool("isDefend", false);
        animator.SetBool("getHit", false);
    }
}
