using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public GameObject Fade;
    public CanvasGroup canvasGroup;
    public bool opening;
    public bool closed;
    public bool moved;
    public float fadeSpeed;
    public RectTransform uiElement;
    public Vector2 startPos = new Vector2(-1600, 1000);
    public Vector2 endPos = new Vector2(1600, -700);
    public float duration = 0.1f; // 過渡時間
    public float delayTime = 1.5f;
    // Start is called before the first frame update
    
    void Start()
    {
        opening = true;
        canvasGroup.alpha = 1.0f;
        fadeSpeed = 0.01f;
        closed = false;
        moved = false;
        delayTime = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            Opening();
        }
        if (closed)
        {
            Closed();
        }
    }
    public void StartMove()
    {
        StopAllCoroutines(); // 防止多次呼叫導致異常
        StartCoroutine(MoveUI(uiElement, startPos, endPos, duration));
    }
    IEnumerator MoveUI(RectTransform target, Vector2 start, Vector2 end, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time+delayTime)
        {
            if (elapsedTime >= delayTime)
            {
                target.anchoredPosition = Vector2.Lerp(start, end, (elapsedTime - delayTime) / time);
                closed = true;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.anchoredPosition = end; // 確保到達最終位置
    }
   void Closed()
    {
        Fade.SetActive(true);
        opening = false;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, fadeSpeed);
        if (canvasGroup.alpha >= 0.995f)
        {
            canvasGroup.alpha = 1;
            closed = false;
        }
    }
    void Opening()
    {
        Fade.SetActive(true);
        closed = false;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, fadeSpeed);
        // canvasGroup.alpha -= canvasGroup.alpha * Time.deltaTime;
        Debug.Log(canvasGroup.alpha);
        if (canvasGroup.alpha <= 0.05)
        {
            canvasGroup.alpha = 0;
            opening = false;
            moved = false;
            Fade.SetActive(false);
        }
    }



}
