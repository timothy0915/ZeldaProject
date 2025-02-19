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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 當玩家按下 F 鍵且在觸發範圍內時
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            // 如果對話框已經顯示，則關閉對話框
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else // 否則，打開對話框並顯示對話內容
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog; // 設置對話框內的文本
            }
        }
    }
    // 當有物件進入觸發區域時調用
    private void OnTriggerEnter(Collider other)
    {
        // 如果進入觸發區域的物件標籤為 Player，則設定 playerInRange 為 true
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // 當物件離開觸發區域時調用
    private void OnTriggerExit(Collider other)
    {
        // 如果離開觸發區域的物件標籤為 Player，則設定 playerInRange 為 false
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
