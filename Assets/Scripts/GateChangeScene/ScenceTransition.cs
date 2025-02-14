using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public string targetSceneName; // �ؼг����W��
    public Vector3 targetPosition; // �ؼЦ�m

    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�I��������O�_�O Player
        if (other.CompareTag("Player"))
        {
            // �[���ؼг���
            SceneManager.LoadScene(targetSceneName);

            // �b�����[����A�N Player ���ʨ���w��m
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ��� Player ����
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // �N Player ���ʨ���w��m
            player.transform.position = targetPosition;
        }

        // �����q�\�ƥ�A�קK���ư���
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}