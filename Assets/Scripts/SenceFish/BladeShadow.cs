using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeShadow : MonoBehaviour
{
    public TrailRenderer swordTrail;
    public float trailDuration = 0.5f; // �C�v�����ɶ�
    public PlayerController playerController;
    void Start()
    {
        swordTrail.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerController.isGrounded) // ���a���U������
        {
            swordTrail.enabled = true;

            // �������e�������p��
            CancelInvoke("DisableTrail");

            // ���s�Ұʷs�������p��
            Invoke("DisableTrail", trailDuration);
        }
    }

    void DisableTrail()
    {
        swordTrail.enabled = false;
    }

}
