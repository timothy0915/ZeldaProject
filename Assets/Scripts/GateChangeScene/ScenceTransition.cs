using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetSceneName; // �ؼг����W��
    public Vector3 targetPosition; // �ؼЦ�m

    FadeInOut fade;//�I�s�H�J�H�X�����

    private void Start()
    {
        //�M���������FadeInOut�ե�
        fade = FindObjectOfType<FadeInOut>();
    }

    public IEnumerator ChangeScene()
    {
        //�}�l�H�J���ĪG
        fade.FadeIn();
        //����1��A�T�O�H�J����
        yield return new WaitForSeconds(1);
        // �s�x Player ��m
        PlayerPrefs.SetFloat("TargetX", targetPosition.x);
        PlayerPrefs.SetFloat("TargetY", targetPosition.y);
        PlayerPrefs.SetFloat("TargetZ", targetPosition.z);
        PlayerPrefs.SetInt("HasSavedPosition", 1); // �аO���w�x�s
        PlayerPrefs.Save(); // �T�O�ƾڳQ�O�s

        // �[���ؼг���
        SceneManager.LoadScene(targetSceneName);

        // �q�\ SceneManager �ƥ�A�T�O�������J�����Ჾ�� Player
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�O�_�O Player �i�J�ǰe�I
        if (other.CompareTag("Player"))
        {
            // �}�l��������
            StartCoroutine(ChangeScene());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���������� Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && PlayerPrefs.GetInt("HasSavedPosition", 0) == 1)
        {
            // Ū���x�s�� Player ��m
            float x = PlayerPrefs.GetFloat("TargetX");
            float y = PlayerPrefs.GetFloat("TargetY");
            float z = PlayerPrefs.GetFloat("TargetZ");

            // �]�w Player ��m
            player.transform.position = new Vector3(x, y, z);

            UnityEngine.Debug.Log($"���a���ʨ�s��m: {player.transform.position}");

            // ���m�s�ɡA�קK�U���}�����ɦA����
            PlayerPrefs.SetInt("HasSavedPosition", 0);
            PlayerPrefs.Save();
        }
        else
        {
            player.transform.position = new Vector3(32.05f, 0f, -32.51f);

        }
       // UnityEngine.Debug.Log(player.name + " OnSceneLoaded : " + player.transform.position);
        // �����q�\�A����h��Ĳ�o
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
