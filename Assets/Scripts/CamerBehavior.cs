using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public float sensitiviy_x = 2f;
    public float sensitiviy_y = 2f;
    public Vector3 CamOffset = new Vector3(0f, 20f, -5f);
    public Transform _target;
    float CameraToTargetDistance = 35f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (CamOffset != new Vector3(0f, 20f, -5f))
        {
            CamOffset = new Vector3(0f, 20f, -5f);
        }
    }

    void LateUpdate()
    {
        if (_target == null) return;
      
        // **保持攝影機跟隨 Player**
        transform.LookAt(_target.position + Vector3.up * 1.5f);
        transform.position = new Vector3(0, 0, -CameraToTargetDistance) + _target.position + Vector3.up * CamOffset.y;
    }
}
