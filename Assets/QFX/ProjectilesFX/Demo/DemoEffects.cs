using System;
using UnityEngine;

public class DemoEffects : MonoBehaviour
{
    public GameObject[] Effects;

    private int _num;

    private void Start()
    {
        UpdateEffects();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            _num++;
            UpdateEffects();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            _num--;
            UpdateEffects();
        }
    }

    private void UpdateEffects()
    {
        if (_num >= Effects.Length)
            _num = 0;
        else if (_num < 0)
            _num = Effects.Length - 1;

        foreach (var effect in Effects)
            effect.SetActive(false);
        
        Effects[_num].SetActive(true);
    }
}
