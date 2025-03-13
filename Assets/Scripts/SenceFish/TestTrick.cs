using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrick : MonoBehaviour
{
    //試驗用 X自殘 Y治療 Z加最大血量
    public HealthBarController healthBarController;
    public HealthBarHUDTester healthBarHUDTester;
    void Update()
    {
        KeyInput();
    }
    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            healthBarHUDTester.Hurt(1);
            Debug.Log("Hurt");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            healthBarHUDTester.Heal(1);
            Debug.Log("Heal");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            healthBarHUDTester.AddHealth();
            Debug.Log("AddHealth");
        }
    }
}
