using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateChangeScene : MonoBehaviour
{
    // ��g�Q�n�[���������W��(�bunity3d��Inspector�̶�g)
    public string sceneToLoad;

    //Box Collider��Is Trigger�O�o�n��
    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�I������H�O�_�O���a
        if (other.CompareTag("Player"))
        {
            // �O���ܡA�[���s����
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
