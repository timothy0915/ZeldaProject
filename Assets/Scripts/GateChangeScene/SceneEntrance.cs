using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    public string lastExitName;// �Ψ��x�s���a�̫����}�������W��
    // Start is called before the first frame update
    void Start()
    {
        // �ˬd�x�s�� "LastExitName" �O�_�P�ثe������ "lastExitName" �ۦP
        if (PlayerPrefs.GetString("LastExitName")==lastExitName)
        {           
            // �p�G�ۦP�A�N���a����m�]�w�������󪺦�m
            PlayerScript.instance.transform.position = transform.position;
            // �N���a�����׳]�w�������󪺨���
            PlayerScript.instance.transform.eulerAngles = transform.eulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
