using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAsp : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        //����]�m��e�̹�����v
        Resolution[] resolutions =Screen.resolutions;
        //�]�m��e����v
        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
        Screen.fullScreen = true;
    }
}
