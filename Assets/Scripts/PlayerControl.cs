using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animator;
    //移動速度
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;

    private float _vInput;
    private float _hInput;

    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;
        
        this.transform.Translate(Vector3.forward*_vInput*Time.deltaTime);
        this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);
      
        //如果輸入的是W
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("MoveW", true);
        }
        //如果輸入的是S
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("MoveS", true);
        }
        //如果輸入的是A
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("MoveA", true);
        }
        //如果輸入的是D
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("MoveD", true);
        }
    }
}
