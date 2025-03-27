using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemageNum : MonoBehaviour
{
    public GameObject numberPrefab; // ���w�@�ӱa�� TMP_Text ���w�s��

    public void SpawnNumber(Vector3 position, int value)
    {
        GameObject numberObj = Instantiate(numberPrefab, position, Quaternion.identity);
        TMP_Text text = numberObj.GetComponent<TMP_Text>();
        text.text = value.ToString();

        // ���Ʀr�y�L�W�ɨòH�X
        Destroy(numberObj, 1.5f);
    }
}
