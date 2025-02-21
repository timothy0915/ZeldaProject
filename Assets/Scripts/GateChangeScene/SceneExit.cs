using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
    public string sceneToLoad;// �n�[���������W��
    public string exitName; // ��e�X�f���W��

    private void OnTriggerEnter(Collider other) // �ˬd�i�JĲ�o�ϰ쪺�O���O���a
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // �x�s��e�X�f���W�١A�o�˥i�H�b���a�i�J�U�@�����ɪ��D�L�Ӧۭ���
            PlayerPrefs.SetString("LastExitName", exitName);
            // �[�����w������
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
