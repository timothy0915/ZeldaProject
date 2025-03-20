using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeShadow : MonoBehaviour
{
    public TrailRenderer swordTrail;
    public float trailDuration = 0.5f; // 劍影維持時間
    public PlayerController playerController;
    void Start()
    {
        swordTrail.enabled = false;
    }

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

}
