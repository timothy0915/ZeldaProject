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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ���a���U F ��B�bĲ�o�d�򤺮�
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            // �p�G��ܮؤw�g��ܡA�h������ܮ�
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else // �_�h�A���}��ܮب���ܹ�ܤ��e
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog; // �]�m��ܮؤ����奻
            }
        }
    }
    // ������i�JĲ�o�ϰ�ɽե�
    private void OnTriggerEnter(Collider other)
    {
        // �p�G�i�JĲ�o�ϰ쪺������Ҭ� Player�A�h�]�w playerInRange �� true
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // �������}Ĳ�o�ϰ�ɽե�
    private void OnTriggerExit(Collider other)
    {
        // �p�G���}Ĳ�o�ϰ쪺������Ҭ� Player�A�h�]�w playerInRange �� false
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
