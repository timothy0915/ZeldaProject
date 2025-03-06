using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attackradius : MonoBehaviour
{
    public float attackRadius;
    public Transform homePosition;public Transform target;  // �l�ܪ��a
    public float chaseRadius = 10f;
    public float moveSpeed = 3f; //�Ǫ����ʳt��

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;  //�T�O��쪱�a
        }
        else
        {
            Debug.LogError("�䤣����Ҭ� 'Player' ������I");
        }
    }

    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (target != null) // �T�O target �s�b�A�קK NullReferenceException
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius) // �ץ����~�� if ����
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}