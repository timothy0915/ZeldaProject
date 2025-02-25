using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInSecond : MonoBehaviour
{
    int i;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
       i=  Timer.GetTimer.GetTimeI();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (i != Timer.GetTimer.GetTimeI())
        {
            anim.speed = Random.Range(1, 10) / 10;
            i = Timer.GetTimer.GetTimeI();
        }
    }
}
