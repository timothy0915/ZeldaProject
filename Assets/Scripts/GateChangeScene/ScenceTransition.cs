using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public string targetSceneName; // 目標場景名稱
    public Vector3 targetPosition; // 目標位置

    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰撞的物件是否是 Player
        if (other.CompareTag("Player"))
        {
            // 加載目標場景
            SceneManager.LoadScene(targetSceneName);

            // 在場景加載後，將 Player 移動到指定位置
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 找到 Player 物件
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 將 Player 移動到指定位置
            player.transform.position = targetPosition;
        }

        // 取消訂閱事件，避免重複執行
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}