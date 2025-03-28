using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemageNum : MonoBehaviour
{
    public static DemageNum Instance { get; private set; }
    public GameObject numberPrefab; // 指定一個帶有 TMP_Text 的預製物

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnNumber(Vector3 position, int value)
    {
        if (numberPrefab == null)
        {
            Debug.LogError("numberPrefab 尚未設置！");
            return;
        }

        GameObject numberObj = Instantiate(numberPrefab, position, Quaternion.identity);
        TMP_Text text = numberObj.GetComponent<TMP_Text>();
        if (text != null)
        {
            text.text = value.ToString();
        }
        else
        {
            Debug.LogError("numberPrefab 上找不到 TMP_Text 組件！");
        }

        // 讓數字稍微上升並淡出
        Destroy(numberObj, 1.5f);
    }
}
