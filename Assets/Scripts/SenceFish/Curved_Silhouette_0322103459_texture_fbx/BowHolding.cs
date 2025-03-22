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
    public float attackTimer = 0f;         // �Ψӭp�ɧ����N�o���p�ɾ�

    // Start is called before the first frame update
    void Start()
    {
        bow.SetActive(false);
        attackTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // �Y���a�w���`�A���A�B�z�����J�Ϊ��A��s
        if (playerController.isDead)
        {
            return;
        }
        // �����N�o�G�Y�����p�ɾ��|���k�s�A�h�����֭˼�
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
