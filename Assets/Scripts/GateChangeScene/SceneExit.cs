using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
    public string sceneToLoad;// 要加載的場景名稱
    public string exitName; // 當前出口的名稱

    private void OnTriggerEnter(Collider other) // 檢查進入觸發區域的是不是玩家
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 儲存當前出口的名稱，這樣可以在玩家進入下一場景時知道他來自哪裡
            PlayerPrefs.SetString("LastExitName", exitName);
            // 加載指定的場景
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
