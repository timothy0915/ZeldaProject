using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public string targetSceneName;
    public void ChangSence()
    {
        SceneManager.LoadScene(targetSceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


}
