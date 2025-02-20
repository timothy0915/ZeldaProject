using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
   public GameObject player;
    public GameObject[] SpwanPoints = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Spwan(int P) 
    {
        player.transform.position = SpwanPoints[P].transform.position;
    }
   public void Teleport(float x,float y, float z)
    {
        player.transform.position = new Vector3(x,y,z);
    }

}
