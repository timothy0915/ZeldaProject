using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class BladeShadow : MonoBehaviour
{
    public TrailRenderer swordTrail;
    public float trailDuration = 0.5f; // 劍影維持時間
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
            if (Input.GetKeyDown(KeyCode.Mouse0) && playerController.isGrounded) // 當玩家按下攻擊鍵
        {
            swordTrail.enabled = true;

            // 取消之前的關閉計時
            CancelInvoke("DisableTrail");

            // 重新啟動新的關閉計時
            Invoke("DisableTrail", trailDuration);
        }
    }

    void DisableTrail()
    {
        swordTrail.enabled = false;
    }
    */
}
