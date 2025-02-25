using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    public BoxCollider boxCollider;
    int timerI;
    float FlyTime = 5f;
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
            Vector3 moveDirection = transform.forward; // 劍的前進方向
            transform.position += moveDirection * 10f * Time.deltaTime;
            // 讓劍的 Z 軸對準前進方向
            transform.rotation = Quaternion.LookRotation(moveDirection);
            if (Timer.GetTimer.timer_i - timerI >= FlyTime)
            {
                timerI = 0;
                this.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("Enemy") || other.CompareTag("Rock"))
        {
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            this.gameObject.SetActive(false);
        }
    }
}
