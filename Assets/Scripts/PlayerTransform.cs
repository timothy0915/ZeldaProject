using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
   public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   void Teleport(float x,float y, float z)
    {
      
        playerTransform.position = new Vector3(x,y,z);

    }

}
