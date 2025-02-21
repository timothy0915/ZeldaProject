using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    public string lastExitName;// 用來儲存玩家最後離開的場景名稱
    // Start is called before the first frame update
    void Start()
    {
        // 檢查儲存的 "LastExitName" 是否與目前場景的 "lastExitName" 相同
        if (PlayerPrefs.GetString("LastExitName")==lastExitName)
        {           
            // 如果相同，將玩家的位置設定為此物件的位置
            PlayerScript.instance.transform.position = transform.position;
            // 將玩家的角度設定為此物件的角度
            PlayerScript.instance.transform.eulerAngles = transform.eulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
