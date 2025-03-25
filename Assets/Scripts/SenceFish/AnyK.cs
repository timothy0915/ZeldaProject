using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyK : MonoBehaviour
{
   public CanvasGroup canvasGroup;
    public bool plus;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup != null)
        {
            if (canvasGroup.alpha >= 0.99f)
            {
                plus = false;
            }
            else if (canvasGroup.alpha <=0.6)
            {
                plus = true;
            }
                
          if (plus)
            {
                canvasGroup.alpha +=Time.deltaTime/ speed;
            }
          else 
            {
                canvasGroup.alpha -= Time.deltaTime/speed;
            }

        }
    }
}
