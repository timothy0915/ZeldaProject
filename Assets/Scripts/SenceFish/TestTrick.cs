using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrick : MonoBehaviour
{
    // Start is called before the first frame update
    public HealthBarController healthBarController;
    public HealthBarHUDTester healthBarHUDTester;
    void Start()
    {
        
    }

    // Update is called once per frame
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
