using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSword : MonoBehaviour
{
    public BoxCollider boxCollider;
    int timerI;
    float flyTime = 5f;
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
            this.transform.position += transform.forward * speed * Time.deltaTime;

            if (Timer.GetTimer.timer_i - timerI >= flyTime)
            {
                timerI = 0;
                this.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 只影響標籤為 "Enemy" 或 "Rock" 的物體
        if (other.CompareTag("Enemy") || other.CompareTag("Trap"))
        {
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
    }
}
