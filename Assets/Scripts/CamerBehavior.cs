using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
//    public float sensitiviy_x = 2f;
//    public float sensitiviy_y = 2f;
    public Vector3 CamOffset = new Vector3(0f, 20f, -5f);
    public Transform _target;
    float CameraToTargetDistance = 35f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CamOffset = new Vector3(0f, 20f, -5f);
        transform.LookAt(_target.position + Vector3.up * 1.5f);
    }

    void LateUpdate()
    {/*
        if (_target == null) return;
      
        // **保持攝影機跟隨 Player**
        transform.LookAt(_target.position + Vector3.up * 1.5f);
        transform.position = new Vector3(0, 0, -CameraToTargetDistance) + _target.position + Vector3.up * CamOffset.y;
    */
        // 設定偏移角度（左右 30 度）
        Quaternion offsetRotation = Quaternion.Euler(0, -15, 0);
        Vector3 rotatedOffset = offsetRotation * new Vector3(0, 0, -CameraToTargetDistance);

        // 計算攝影機位置
        transform.position = _target.position + rotatedOffset + Vector3.up * CamOffset.y;

        // 保持攝影機朝向 Player
        transform.LookAt(_target.position + Vector3.up * 1.5f);
    }
}
