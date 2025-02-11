using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CamerBehavior : MonoBehaviour
{
    public Vector3 CamOffest = new Vector3(0f, 1.2f, -2.6f);
    private Transform _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player").transform;
    }
     void LateUpdate()
    {
    this.transform.position=_target.TransformPoint(CamOffest);
        this.transform.LookAt(_target);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
