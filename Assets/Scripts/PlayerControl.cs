using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animator;
    //移動速度
    public float MoveSpeed = 10f;
    //奔跑速度加乘
    [Range(1, 3)] public float SprintSpeedModifier = 2;
    //旋轉速度
    public float RotateSpeed = 75f;
    //重力
    private float gravity = 20f;
    //與地面的距離
    private float DistanceToGround = 0.1f;
    //加速度百分比
    public float addSpeedRatio = 0.05f;

    private bool isRun = false;

    //主體的Vector3方位
    public Vector3 Velocity;
    //每一Frame要移動到的目標位置
    Vector3 TargetMovement;
    //上一Frame的移動速度
    float lastFrameSpeed;

    CharacterController _cc;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        MoveBehaviour();
    }

    Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        return Vector3.ClampMagnitude(move, 1);
    }
    void MoveBehaviour()
    {
        TargetMovement = Vector3.zero;
        TargetMovement += GetMoveInput().z * Vector3.forward;
        TargetMovement += GetMoveInput().x * Vector3.right;


        //避免對角線超過1
        TargetMovement = Vector3.ClampMagnitude(TargetMovement, 1);

        //下一Frame的移動速度
        float nextFrameSpeed = 0;

        if (TargetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0f;
            animator.SetFloat("MoveSpeed", 0f);
        }
        else
        {
            SmoothRotation(TargetMovement);
            animator.SetFloat("MoveSpeed", 0.5f);
        }
        
        if (IsRun())
        {
            nextFrameSpeed = 1f;
            TargetMovement *= SprintSpeedModifier;
            SmoothRotation(TargetMovement);
        }

        if(lastFrameSpeed != nextFrameSpeed)
        {
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);
        }
        _cc.Move(TargetMovement * Time.deltaTime * MoveSpeed);
    }

    void SmoothRotation(Vector3 TargetMovement)
    {
        if(TargetMovement.sqrMagnitude > 0.01f)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(TargetMovement,Vector3.up), RotateSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (_cc.isGrounded)
        {
            Velocity.y = -0.05f;
        }
        else
        {
            Velocity.y -= gravity * Time.deltaTime;
        }
        _cc.Move(Velocity * Time.deltaTime);
    }

    
    
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, DistanceToGround);
    }
    

    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        return cameraForward;
    }
    private Vector3 GetCurrentCameraRight()
    {
        Vector3 CameraRight = Camera.main.transform.right;
        CameraRight.y = 0f;
        CameraRight.Normalize();
        return CameraRight;
    }

    private bool IsRun()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
