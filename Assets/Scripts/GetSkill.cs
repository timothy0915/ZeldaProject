using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���}���Ω�B�z���a����ޯ�θɵ��~�]�Ҧp�_�c�^�ɪ��欰
public class GetSkill : MonoBehaviour
{
    public GameObject dialogBox;      // ��ܮت���A�Ω���ܩ����ô��ܰT��
    public Text dialogText;           // ��ܮؤ���ܪ���r
    public string dialog;             // �n��ܪ���ܤ��e
    public bool playerInRange;        // �P�_���a�O�_�i�JĲ�o�d��

    // �_�c���ʵe����A�����b Inspector �����w������ Animator
    public Animator chestAnimator;

    // �̤j��q�ȡA�o�̰��]���a���̤j��q�� 100f
    public float maxHealth = 100f;

    // Update() �C�@�V���|�Q�I�s�A��ť���a����ާ@
    void Update()
    {
        // ���a���U X ��B�bĲ�o�d�򤺮ɰ���
        if (Input.GetKeyDown(KeyCode.X) && playerInRange)
        {
            // ������ܮت���ܪ��A�]�p�G�w��ܫh���áA�Ϥ���M�^
            dialogBox.SetActive(!dialogBox.activeInHierarchy);
            if (dialogBox.activeInHierarchy)
            {
                // �p�G��ܮسQ��ܡA�]�w�䤤����r���e
                dialogText.text = dialog;
            }

            // Ĳ�o�_�c���}�Ұʵe�A�нT�O Animator ���������� "OpenChest" Trigger
            if (chestAnimator != null)
            {
                chestAnimator.SetTrigger("OpenChest");
            }

            // ��쪱�a����A�ھڼ��� "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // ���o���a�� PlayerController �}���A�ñN��q��_��̤j��
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.health = maxHealth;
                }
            }
        }
    }

    // ������i�JĲ�o�ϰ�ɡA�|�I�s����k
    private void OnTriggerEnter(Collider other)
    {
        // �p�G�i�J������аO�� "Player"�A�h�]�w playerInRange �� true
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // �������}Ĳ�o�ϰ�ɡA�|�I�s����k
    private void OnTriggerExit(Collider other)
    {
        // �p�G���}������аO�� "Player"�A�h�]�w playerInRange �� false
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // ���a���}�ɡA�۰�������ܮ�
            dialogBox.SetActive(false);
        }
    }
}