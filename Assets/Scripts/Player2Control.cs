using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Player2Control : MonoBehaviour
{
    private AnimatorCtrl animatorCtrl;
    public float MoveSpeed = 5f;
    [Range(1, 3)] public float SprintSpeedModifier = 2;
    public float RotateSpeed = 75f;
    private float gravity = 15f;
    public float addSpeedRatio = 0.05f;
    public Vector3 Velocity;
    float lastFrameSpeed;
    public Vector3 movement;
    CharacterController controller;
    bool bInitFirst = false;

    int hit = 0;
    float hitTime = 0f;
    float focusTime = 0f;
    int spinTime = 0;
    float pastTime = 0;
    public float speed = 5f;
    public float rotationSpeed = 720f; // ����t��
    public bool canRotate = true; // �O�_���\����

    private Vector3 moveDirection;
    public float jumpHeight = 4f;
    public GameObject shadow;
    public GameObject ShotSword;
    void Start()
    {
        speed = 5f;
        animatorCtrl = GetComponent<AnimatorCtrl>();
        Velocity = Vector3.zero;
    // **Ū�������������x�s�����a��m**
    //if (PlayerPrefs.GetInt("HasSavedPosition", 0) == 1)
    //{
    //    float x = PlayerPrefs.GetFloat("TargetX");
    //    float y = PlayerPrefs.GetFloat("TargetY");
    //    float z = PlayerPrefs.GetFloat("TargetZ");

    //    transform.position = new Vector3(x, y, z);
    //    UnityEngine.Debug.Log($"���a�_�l��m: {transform.position}");

    //    // ���m�s�ɡA�קK�v�T���᪺����
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
            UnityEngine.Debug.Log($"���a�_�l��m: {transform.position}");
        }
        bInitFirst = true;
    }

    private void Awake()
    {
        //   animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyGravity();
        MaybeRo();
        Dash();
        ActionApplied();
        //Jump();
        ApplyMove();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))//���U����(�榸Ĳ�o
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
            //   Debug.Log("�b"+ pastTime + "��᪺��" + hit + "��");
            hitTime = focusTime = Timer.GetTimer.GetTimeF();
        }
        if (Input.GetKey(KeyCode.Mouse0))//������(����Ĳ�o
        {
            pastTime = Timer.GetTimer.GetTimeI() - focusTime + 0.7f;
            if (pastTime >= 1)
            {
                speed = 2f;
                animatorCtrl.Focus(true);
                
                animatorCtrl.Attack(false, hit);
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))//��}����
        {
            pastTime = Timer.GetTimer.GetTimeI() - focusTime;
            if (pastTime >= 2)
            {
                //animatorCtrl.Spin(true);
                //spinTime = 200;
                if (ShotSword != null)
                {
                    ShotSword.transform.position = this.transform.position+transform.forward;
                    ShotSword.transform.rotation = this.transform.rotation;
                    ShotSword.SetActive(true);
                }
            }
            speed = 5f;
            animatorCtrl.Focus(false);
            animatorCtrl.Attack(false, hit);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))//���U�k��
        {
            speed = 2f;
             canRotate = false;
            animatorCtrl.Defend(true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))//��}�k��
        {
            speed = 5f;
            canRotate = true;
            animatorCtrl.Defend(false);
        }
        
    }
    void Jump()
    {
        UnityEngine.Debug.Log("jumpUnity"+ controller.isGrounded);
        if (Input.GetKey(KeyCode.Space) && controller.isGrounded)
        {

            Velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
           UnityEngine.Debug.Log("jump");
            animatorCtrl.Jump(true); 
        }
    }
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (shadow != null)
            {
                shadow.transform.position = this.transform.position;
                shadow.transform.rotation = this.transform.rotation;
                shadow.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
    }
    void MaybeRo()
    {
        // ���o�����P������J (WASD �Τ�V��)

        // �p�Ⲿ�ʤ�V
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        float nextFrameSpeed = 0;

        if (moveDirection == Vector3.zero)
        {
            nextFrameSpeed = 0f;
            animatorCtrl.Move(0f);
        }
        else
        {
            // **�ץ��G�� canRotate �� false�A���n�I�s SmoothRotation**
            if (canRotate)
            {
                SmoothRotation(moveDirection);
            }
            animatorCtrl.Move(0.5f);
        }

        if (lastFrameSpeed != nextFrameSpeed)
        {
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);
        }

        // �����Ⲿ��
        if (moveDirection.magnitude >= 0.1f)
        {
            movement += (moveDirection * speed * Time.deltaTime);

            // **�ץ��G�u���� canRotate �� true �ɡA����~�|��V**
            if (canRotate)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    void SmoothRotation(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), RotateSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        //UnityEngine.Debug.Log("it's Ground0" + controller.isGrounded);
        if (controller.isGrounded)
        {
            Velocity.y = -0.001f;
            animatorCtrl.Jump(false);
            animatorCtrl.Ground(true);
        }
        else
        {
            Velocity.y -= gravity * Time.deltaTime;
            
            movement += (Velocity * Time.deltaTime);
        }
      //  UnityEngine.Debug.Log(Velocity);

        
        //UnityEngine.Debug.Log("it's Ground" +controller.isGrounded);
    }
    void ApplyMove()
    {
        controller.Move(movement);
        movement = Vector3.zero;
    }
}
