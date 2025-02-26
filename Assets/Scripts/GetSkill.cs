using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSkill : MonoBehaviour
{
    public GameObject dialogBox; // 用於顯示或隱藏對話框
    public Text dialogText; // 對話框內的文本顯示
    public string dialog; // 儲存要顯示的對話內容
    public bool playerInRange; // 判斷玩家是否進入可觸發範圍
    // Start is called before the first frame update
    void Update()
    {
        // 當玩家按下 X 鍵且在觸發範圍內時
        if (Input.GetKeyDown(KeyCode.X) && playerInRange)
        {
            // 切換對話框的顯示狀態
            dialogBox.SetActive(!dialogBox.activeInHierarchy);
            if (dialogBox.activeInHierarchy)
            {
                dialogText.text = dialog; // 設置對話框內的文本
            }
        }
    }

    // 當有物件進入觸發區域時調用
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // 當物件離開觸發區域時調用
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false); // 玩家離開時關閉對話框
        }
    }
}
