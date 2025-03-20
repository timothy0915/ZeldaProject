using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BladeShadow : MonoBehaviour
{
    public TrailRenderer swordTrail;
    public float trailDuration = 0.5f; // �C�v�����ɶ�
    public PlayerController playerController;
    public Animator animator;
    public float frame01;
    public float frame02;
    void Start()
    {
        swordTrail.enabled = false;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack01_SwordAndShiled")) attack01();

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack02_SwordAndShiled")) attack02();
    }
  void attack01()
    {
        frame02 = 0;
        frame01 += Time.deltaTime;
        if (frame01 > 0.1f && frame01 < 0.22f) swordTrail.enabled = true;
        else swordTrail.enabled = false;
        Debug.Log("frame="+ frame01);
    }

    void attack02()
    {
        frame01 = 0;
        frame02 += Time.deltaTime;
        if (frame02 > 0.1f && frame02 < 0.22f) swordTrail.enabled = true;
        else swordTrail.enabled = false;
        Debug.Log("frame=" + frame02);
    }

    /*
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
    */
}
