using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemageNum : MonoBehaviour
{
    public static DemageNum Instance { get; private set; }
    public GameObject numberPrefab; // ���w�@�ӱa�� TMP_Text ���w�s��

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
            Debug.LogError("numberPrefab �|���]�m�I");
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
            Debug.LogError("numberPrefab �W�䤣�� TMP_Text �ե�I");
        }

        // ���Ʀr�y�L�W�ɨòH�X
        Destroy(numberObj, 1.5f);
    }
}
