using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer GetTimer;
    public float timer_f = 0f;
    public int timer_i = 0;
    private void Awake()
    {
        // �T�O�u�s�b�@�� Timer
        if (GetTimer == null)
            GetTimer = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        timer_f += Time.deltaTime;
        if (timer_f - 1 > timer_i)
        {
            timer_i = (int)timer_f;
            //Debug.Log(timer_i);Ū��
        }
    }
    public float GetTimeF()
    {
        return timer_f;
    }
    public int GetTimeI()
    {
        return timer_i;
    }
    
}
