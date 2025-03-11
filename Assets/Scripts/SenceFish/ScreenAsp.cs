using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAsp : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        //獲取設置當前屏幕分辨率
        Resolution[] resolutions =Screen.resolutions;
        //設置當前分辨率
        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
        Screen.fullScreen = true;
    }
}
