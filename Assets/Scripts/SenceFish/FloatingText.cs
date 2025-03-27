using TMPro;
using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f;  // 上升速度
    public float fadeOutTime = 1f; // 淡出時間

    private TMP_Text textMesh;
    private Color startColor;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        startColor = textMesh.color;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        Vector3 startPosition = transform.position;

        while (elapsedTime < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeOutTime);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            transform.position = startPosition + new Vector3(0, moveSpeed * elapsedTime, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
