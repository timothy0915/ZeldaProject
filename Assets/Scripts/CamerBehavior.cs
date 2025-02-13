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
    public Vector3 CamOffest = new Vector3(0f, 1.2f, -2.6f);
    public Transform _target;

    //�ƹ��̧C�i�ݨ���
    float MinVerticalAngle = 40f;
    //�ƹ��̰��i�ݨ���
    float MaxVeticalAngle = 70;
    //��v���Z���ؼЪ���m
    float CameraToTargetDistance = 10f;
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
        mouse_x += GetMouseXAxis() * sensitiviy_x;
        mouse_y -= GetMouseYAxis() * sensitiviy_y;
        mouse_y = Mathf.Clamp(mouse_y, MinVerticalAngle, MaxVeticalAngle);

        transform.rotation = Quaternion.Euler(mouse_y, mouse_x, 0f);
        transform.position = Quaternion.Euler(mouse_y, mouse_x, 0) * new Vector3(0, 0, -CameraToTargetDistance) + _target.position + Vector3.up * CamOffest.y;
    }


    public bool CanProcessInput() => Cursor.lockState == CursorLockMode.Locked;
    public float GetMouseXAxis() => CanProcessInput() ? Input.GetAxis("Mouse X") : 0f;
    public float GetMouseYAxis() => CanProcessInput() ? Input.GetAxis("Mouse Y") : 0f;
}
