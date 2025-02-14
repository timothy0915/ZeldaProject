using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CamerBehavior : MonoBehaviour
{
    //�����F�ӫ�
    public float sensitiviy_x = 2f;
    //�����F�ӫ�
    public float sensitiviy_y = 2f;
    public Vector3 CamOffest = new Vector3(0f, 2f, -5f);
    public Transform _target;

    //�ƹ��̧C�i�ݨ���
    float MinVerticalAngle = 30f;
    //�ƹ��̰��i�ݨ���
    float MaxVeticalAngle = 80f;
    //��v���Z���ؼЪ���m
    float CameraToTargetDistance = 35f;
    float mouse_x = 0f;
    float mouse_y = 30f;
    Vector3 smoothVelocity = Vector3.zero;

    private PlayerControl playerControl;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
    }
    void LateUpdate()
    {
        if (_target == null) return;

        // �ϥ� Lerp ���Ʋ���

        transform.LookAt(_target.position + Vector3.up * 1.5f);
        transform.position = new Vector3(0, 0, -CameraToTargetDistance) + _target.position + Vector3.up * CamOffest.y;
        CamOffest = new Vector3(0f, 20f, 0f);
    }


    public bool CanProcessInput() => Cursor.lockState == CursorLockMode.Locked;
    public float GetMouseXAxis() => CanProcessInput() ? Input.GetAxis("Mouse X") : 0f;
    public float GetMouseYAxis() => CanProcessInput() ? Input.GetAxis("Mouse Y") : 0f;
}
