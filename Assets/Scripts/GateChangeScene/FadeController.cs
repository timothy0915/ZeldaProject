using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    FadeInOut fade;// 用於存儲 FadeInOut 類的實例

    // Start is called before the first frame update
    void Start()
    {
        // 在場景中查找FadeInOut類的實例，並將其賦值給 fade 變量
        fade = FindObjectOfType<FadeInOut>();
        // 調用FadeOut方法，使畫面淡出
        fade.FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
