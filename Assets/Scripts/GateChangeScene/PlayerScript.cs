using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // 定義一個靜態的 PlayerScript 實例，這樣可以在其他地方直接引用
    public static PlayerScript instance;
    // Start is called before the first frame update
    void Start()
    {
        // 檢查是否已經有 PlayerScript 實例
        if (instance != null)
        {
            // 如果已經有實例，則銷毀當前的物件
            Destroy(gameObject);
        }
        else
        {
            // 如果沒有實例，則設置 instance 為當前物件
            instance = this;
        }
        // 讓這個物件在場景切換後不被銷毀
        DontDestroyOnLoad(gameObject);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
