using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    FadeInOut fade;// �Ω�s�x FadeInOut �������

    // Start is called before the first frame update
    void Start()
    {
        // �b�������d��FadeInOut������ҡA�ñN���ȵ� fade �ܶq
        fade = FindObjectOfType<FadeInOut>();
        // �ե�FadeOut��k�A�ϵe���H�X
        fade.FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
