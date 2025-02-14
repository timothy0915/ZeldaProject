using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateChangeScene : MonoBehaviour
{
    // 填寫想要加載的場景名稱(在unity3d的Inspector裡填寫)
    public string sceneToLoad;

    //Box Collider的Is Trigger記得要勾
    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰撞的對象是否是玩家
        if (other.CompareTag("Player"))
        {
            // 是的話，加載新場景
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
