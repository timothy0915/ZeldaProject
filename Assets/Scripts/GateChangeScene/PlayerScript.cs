using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // �w�q�@���R�A�� PlayerScript ��ҡA�o�˥i�H�b��L�a�誽���ޥ�
    public static PlayerScript instance;
    // Start is called before the first frame update
    void Start()
    {
        // �ˬd�O�_�w�g�� PlayerScript ���
        if (instance != null)
        {
            // �p�G�w�g����ҡA�h�P����e������
            Destroy(gameObject);
        }
        else
        {
            // �p�G�S����ҡA�h�]�m instance ����e����
            instance = this;
        }
        // ���o�Ӫ���b���������ᤣ�Q�P��
        DontDestroyOnLoad(gameObject);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
