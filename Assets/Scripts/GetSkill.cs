using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 此腳本用於處理玩家獲取技能或補給品（例如寶箱）時的行為
public class GetSkill : MonoBehaviour
{
    public GameObject dialogBox;      // 對話框物件，用於顯示或隱藏提示訊息
    public Text dialogText;           // 對話框內顯示的文字
    public string dialog;             // 要顯示的對話內容
    public bool playerInRange;        // 判斷玩家是否進入觸發範圍
    public AudioSource audioSource;       // 開箱音效
    public AudioSource musicAudio;          // 在有音效時將音樂聲音調低

    // 寶箱的動畫控制器，必須在 Inspector 中指定對應的 Animator
    public Animator chestAnimator;

    // 最大血量值，這裡假設玩家的最大血量為 100f
    public float maxHealth = 100f;

    // Update() 每一幀都會被呼叫，監聽玩家按鍵操作
    void Update()
    {
        // 當玩家按下 X 鍵且在觸發範圍內時執行
        if (Input.GetKeyDown(KeyCode.X) && playerInRange)
        {
            // 切換對話框的顯示狀態（如果已顯示則隱藏，反之亦然）
            dialogBox.SetActive(!dialogBox.activeInHierarchy);
            if (dialogBox.activeInHierarchy)
            {
                // 如果對話框被顯示，設定其中的文字內容
                dialogText.text = dialog;
            }

            // 觸發寶箱的開啟動畫，請確保 Animator 中有對應的 "OpenChest" Trigger
            if (chestAnimator != null)
            {
                chestAnimator.SetTrigger("OpenChest");
            }

            // 找到玩家物件，根據標籤 "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // 取得玩家的 PlayerController 腳本，並將血量恢復到最大值
                PlayerController playerController = player.GetComponent<PlayerController>();
               
                if (playerController != null)
                {
                    //開啟音效物件並撥放
                    audioSource.enabled = true;
                    audioSource.Play();
                    //降低音樂音量
                    musicAudio.volume = 0.3f;
                    playerController.health = maxHealth;
                }
            }
        }
    }

    // 當有物件進入觸發區域時，會呼叫此方法
    private void OnTriggerEnter(Collider other)
    {
        // 如果進入的物件標記為 "Player"，則設定 playerInRange 為 true
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // 當物件離開觸發區域時，會呼叫此方法
    private void OnTriggerExit(Collider other)
    {
        // 如果離開的物件標記為 "Player"，則設定 playerInRange 為 false
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // 當玩家離開時，自動關閉對話框
            dialogBox.SetActive(false);
            //關閉音效物件
            audioSource.Stop();
            audioSource.enabled = false;
        }
    }
}