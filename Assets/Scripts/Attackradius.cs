using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attackradius : MonoBehaviour
{
    public float attackRadius;
    public Transform homePosition;public Transform target;  // 追蹤玩家
    public float chaseRadius = 10f;
    public float moveSpeed = 3f; //怪物移動速度

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;  //確保找到玩家
        }
        else
        {
            Debug.LogError("找不到標籤為 'Player' 的物件！");
        }
    }

    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (target != null) // 確保 target 存在，避免 NullReferenceException
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius) // 修正錯誤的 if 條件
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}