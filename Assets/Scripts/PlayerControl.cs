using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Animator animator;
    public float MoveSpeed = 10f;
    [Range(1, 3)] public float SprintSpeedModifier = 2;
    public float RotateSpeed = 75f;
    private float gravity = 20f;
    private float DistanceToGround = 0.1f;
    public float addSpeedRatio = 0.05f;
    private bool isRun = false;

    public Vector3 Velocity;
    Vector3 TargetMovement;
    float lastFrameSpeed;

    CharacterController _cc;

    void Start()
    {
        // **讀取場景切換時儲存的玩家位置**
        if (PlayerPrefs.GetInt("HasSavedPosition", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("TargetX");
            float y = PlayerPrefs.GetFloat("TargetY");
            float z = PlayerPrefs.GetFloat("TargetZ");

            transform.position = new Vector3(x, y, z);
            UnityEngine.Debug.Log($"玩家起始位置: {transform.position}");

            // 重置存檔，避免影響之後的場景
            PlayerPrefs.SetInt("HasSavedPosition", 0);
            PlayerPrefs.Save();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
    }

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
        TargetMovement = Vector3.ClampMagnitude(TargetMovement, 1);

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

        if (lastFrameSpeed != nextFrameSpeed)
        {
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);
        }
        _cc.Move(TargetMovement * Time.deltaTime * MoveSpeed);
    }

    void SmoothRotation(Vector3 TargetMovement)
    {
        if (TargetMovement.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(TargetMovement, Vector3.up), RotateSpeed * Time.deltaTime);
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

    private bool IsRun()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
