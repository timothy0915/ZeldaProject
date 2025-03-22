using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHolding : MonoBehaviour
{
    public GameObject bow;
    public GameObject sword;
    public GameObject arrow;
    public Transform shotP;
    public Animator animator;
    public PlayerController playerController;
    public float attackTimer = 0f;         // 用來計時攻擊冷卻的計時器

    // Start is called before the first frame update
    void Start()
    {
        bow.SetActive(false);
        attackTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 若玩家已死亡，不再處理任何輸入或狀態更新
        if (playerController.isDead)
        {
            return;
        }
        // 攻擊冷卻：若攻擊計時器尚未歸零，則持續減少倒數
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if(attackTimer < 0)
        {
            attackTimer = 0;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            sword.SetActive(false);
            bow.SetActive(true);
            animator.SetBool("defend", true);
            if (attackTimer == 0)
            {
                ArrowShot();
                attackTimer = 0.2f;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            bow.SetActive(false);
            sword.SetActive(true);
            animator.SetBool("defend", false);
        }
    }
    void ArrowShot()
    {
        Debug.Log("syuuuu");
        //rand rotation
        int rX = Random.Range(0, 15);
        int rY = Random.Range(0, 15);
        int rZ = Random.Range(0, 15);
        Instantiate(arrow, shotP.position, shotP.rotation*Quaternion.Euler(rX, rY, rZ), shotP);
    }

}
