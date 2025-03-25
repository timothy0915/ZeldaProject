using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOrDie : MonoBehaviour
{
    public GameObject winOrDie;
    public GameObject win;
    public GameObject die;
    public string targetSceneName;
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        winOrDie.SetActive(false);
    }
    public void Win() 
    {
        winOrDie.SetActive(true);
        die.SetActive(false);
        win.SetActive(true);
        ChangScene();
    }

    public void Die()
    {
        winOrDie.SetActive(true);
        win.SetActive(false);
        die.SetActive(true);
        ChangScene();
    }
    public void ChangScene()
    {
        delayTime += Time.deltaTime;
        if (Input.anyKey&& delayTime>=1.5f)
        {
            SceneManager.LoadScene(targetSceneName);//StartGame
        }
    }
}
