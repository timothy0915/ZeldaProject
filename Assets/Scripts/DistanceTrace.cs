using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DistanceTrace : MonoBehaviour
{
    public Transform CamPoint;
    public Transform player;
    private float distance;
    Vector3 direction;
    public float moveSpeed =5f; // 玩家移動速度
    bool ifOut;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        ifOut=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        distance = (player.position - CamPoint.position).magnitude;
        Debug.Log("distance=" + distance);
        if (distance > 5f) ifOut = true;
        if (ifOut)
        { 
        direction = (player.transform.position - CamPoint.transform.position).normalized;
        CamPoint.transform.LookAt(player.transform.position);
        CamPoint.transform.position += direction * moveSpeed * Time.deltaTime;
            if (distance < 2f) ifOut = false;
        }
        
        
    }
}
