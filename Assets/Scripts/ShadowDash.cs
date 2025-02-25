using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowDash : MonoBehaviour
{
    public GameObject player;
    public BoxCollider boxCollider;
    int timerI;
    float dashTime = 1f;
    float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        timerI = 0;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerI == 0) timerI = Timer.GetTimer.timer_i;
       
        if (timerI != 0)
        {
            this.transform.position += transform.forward * speed* Time.deltaTime;

            if (Timer.GetTimer.timer_i - timerI >= dashTime)
            {
                timerI = 0;
                DashOver();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("Wall") || other.CompareTag("Tree") )
        {
           this.transform.position += new Vector3(0,speed* Time.deltaTime,0);
        }
    }
    void DashOver()
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.SetActive(true);
        this.gameObject.SetActive(false);
           
    }
}
