using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetSceneName; // 目標場景名稱
    public Vector3 targetPosition; // 目標位置

    FadeInOut fade;//呼叫淡入淡出的控制器

    private void Start()
    {
        //尋找場景中的FadeInOut組件
        fade = FindObjectOfType<FadeInOut>();
    }

    public IEnumerator ChangeScene()
    {
        //開始淡入的效果
        fade.FadeIn();
        //等待1秒，確保淡入完成
        yield return new WaitForSeconds(1);
        // 存儲 Player 位置
        PlayerPrefs.SetFloat("TargetX", targetPosition.x);
        PlayerPrefs.SetFloat("TargetY", targetPosition.y);
        PlayerPrefs.SetFloat("TargetZ", targetPosition.z);
        PlayerPrefs.SetInt("HasSavedPosition", 1); // 標記為已儲存
        PlayerPrefs.Save(); // 確保數據被保存

        // 加載目標場景
        SceneManager.LoadScene(targetSceneName);

        // 訂閱 SceneManager 事件，確保場景載入完成後移動 Player
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 檢查是否是 Player 進入傳送點
        if (other.CompareTag("Player"))
        {
            // 開始切換場景
            StartCoroutine(ChangeScene());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 找到場景中的 Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && PlayerPrefs.GetInt("HasSavedPosition", 0) == 1)
        {
            // 讀取儲存的 Player 位置
            float x = PlayerPrefs.GetFloat("TargetX");
            float y = PlayerPrefs.GetFloat("TargetY");
            float z = PlayerPrefs.GetFloat("TargetZ");

            // 設定 Player 位置
            player.transform.position = new Vector3(x, y, z);

            UnityEngine.Debug.Log($"玩家移動到新位置: {player.transform.position}");

            // 重置存檔，避免下次開場景時再移動
            PlayerPrefs.SetInt("HasSavedPosition", 0);
            PlayerPrefs.Save();
        }
        else
        {
            player.transform.position = new Vector3(32.05f, 0f, -32.51f);

        }
       // UnityEngine.Debug.Log(player.name + " OnSceneLoaded : " + player.transform.position);
        // 取消訂閱，防止多次觸發
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
