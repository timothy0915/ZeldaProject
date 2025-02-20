using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
   private AnimatorCtrl animatorCtrl;
    public float MoveSpeed = 5f;
    [Range(1, 3)] public float SprintSpeedModifier = 2;
    public float RotateSpeed = 75f;
    private float gravity = 20f;
    public float addSpeedRatio = 0.05f;

    public Vector3 Velocity;
    Vector3 TargetMovement;
    float lastFrameSpeed;

    CharacterController _cc;
    bool bInitFirst = false;

    int hit = 0;
    float hitTime = 0f;
    float focusTime = 0f;
    int spinTime = 0;
    float pastTime = 0;
    void Start()
    {
        MoveSpeed = 5f;
        animatorCtrl = GetComponent<AnimatorCtrl>();
        // **讀取場景切換時儲存的玩家位置**
        //if (PlayerPrefs.GetInt("HasSavedPosition", 0) == 1)
        //{
        //    float x = PlayerPrefs.GetFloat("TargetX");
        //    float y = PlayerPrefs.GetFloat("TargetY");
        //    float z = PlayerPrefs.GetFloat("TargetZ");

        //    transform.position = new Vector3(x, y, z);
        //    UnityEngine.Debug.Log($"玩家起始位置: {transform.position}");

        //    // 重置存檔，避免影響之後的場景
        //    PlayerPrefs.SetInt("HasSavedPosition", 0);
        //    PlayerPrefs.Save();
        //}
      //  UnityEngine.Debug.Log(gameObject.name + " start : " + transform.position);
    }

    private void OnEnable()
    {
        if (bInitFirst)
        {
            float x = PlayerPrefs.GetFloat("TargetX");
            float y = PlayerPrefs.GetFloat("TargetY");
            float z = PlayerPrefs.GetFloat("TargetZ");

            transform.position = new Vector3(x, y, z);
            UnityEngine.Debug.Log($"玩家起始位置: {transform.position}");
        }
        bInitFirst = true;
    }

    private void Awake()
    {
     //   animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyGravity();
        MoveBehaviour();
        ActionApplied();
    }
    void ActionApplied()
    {
        if (spinTime >= 1)
        {
            spinTime -= 1;
            if (spinTime <= 0)
            {
                animatorCtrl.Spin(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))//按下左鍵(單次觸發
        {
            pastTime = Timer.GetTimer.GetTimeF() - hitTime;
            if (hit >= 3 || pastTime >= 3f)
            {
                hit = 1;
            }
            else
            {
                hit += 1;
            }
            animatorCtrl.Attack(true, hit);
            //   Debug.Log("在"+ pastTime + "秒後的第" + hit + "擊");
            hitTime = focusTime = Timer.GetTimer.GetTimeF();
        }
        if (Input.GetKey(KeyCode.Mouse0))//按住左鍵(持續觸發
        {
            pastTime = Timer.GetTimer.GetTimeI() - focusTime + 0.7f;
            if (pastTime >= 1)
            {
                MoveSpeed = 2f;
                animatorCtrl.Focus(true);
                animatorCtrl.Attack(false, hit);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))//放開左鍵
        {
            pastTime = Timer.GetTimer.GetTimeI() - focusTime;
            if (pastTime >= 2)
            {
                animatorCtrl.Spin(true);
                spinTime = 200;
            }
            MoveSpeed = 5f;
            animatorCtrl.Focus(false);
            animatorCtrl.Attack(false, hit);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))//按下右鍵
        {
            MoveSpeed = 2f;
            animatorCtrl.Defend(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))//放開右鍵
        {
            MoveSpeed = 5f;
            animatorCtrl.Defend(false);
        }
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
            animatorCtrl.Move(0f);
        }
        else
        {
            SmoothRotation(TargetMovement);
            animatorCtrl.Move(0.5f);
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

    
}
