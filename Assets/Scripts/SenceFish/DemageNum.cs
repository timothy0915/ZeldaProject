using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemageNum : MonoBehaviour
{
    public GameObject numberPrefab; // 指定一個帶有 TMP_Text 的預製物

    public void SpawnNumber(Vector3 position, int value)
    {
        GameObject numberObj = Instantiate(numberPrefab, position, Quaternion.identity);
        TMP_Text text = numberObj.GetComponent<TMP_Text>();
        text.text = value.ToString();

        // 讓數字稍微上升並淡出
        Destroy(numberObj, 1.5f);
    }
}
