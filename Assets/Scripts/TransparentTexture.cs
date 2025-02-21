using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentTexture : MonoBehaviour
{
    public Material material;  // �즲�A�������o��

    void Start()
    {
        if (material != null && material.HasProperty("_MainTex"))
        {
            Texture2D tex = material.mainTexture as Texture2D;
            if (tex != null)
            {
                Color[] pixels = tex.GetPixels();
                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i].a *= 0.3f; // ���C�z���� 30%
                }
                tex.SetPixels(pixels);
                tex.Apply();
            }
        }
    }
}
