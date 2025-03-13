using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public string targetSceneName;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // ¸ÑÂê·Æ¹«
        Cursor.visible = true; // Åã¥Ü·Æ¹«
    }
    public void DelayedChangSence()
    {
        Invoke("ChangSence", 2.1f);
    }
    public void DelayedQuitGame()
    {
        Invoke("QuitGame", 6);
    }
    public void ChangSence()
    {
        SceneManager.LoadScene(targetSceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


}
