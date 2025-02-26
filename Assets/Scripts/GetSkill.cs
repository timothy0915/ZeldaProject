using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSkill : MonoBehaviour
{
    public GameObject dialogBox; // �Ω���ܩ����ù�ܮ�
    public Text dialogText; // ��ܮؤ����奻���
    public string dialog; // �x�s�n��ܪ���ܤ��e
    public bool playerInRange; // �P�_���a�O�_�i�J�iĲ�o�d��
    // Start is called before the first frame update
    void Update()
    {
        // ���a���U X ��B�bĲ�o�d�򤺮�
        if (Input.GetKeyDown(KeyCode.X) && playerInRange)
        {
            // ������ܮت���ܪ��A
            dialogBox.SetActive(!dialogBox.activeInHierarchy);
            if (dialogBox.activeInHierarchy)
            {
                dialogText.text = dialog; // �]�m��ܮؤ����奻
            }
        }
    }

    // ������i�JĲ�o�ϰ�ɽե�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // �������}Ĳ�o�ϰ�ɽե�
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false); // ���a���}��������ܮ�
        }
    }
}
