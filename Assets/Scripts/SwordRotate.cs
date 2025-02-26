using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRotate : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = player.transform.rotation;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x , transform.eulerAngles.y, transform.eulerAngles.z+90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
