using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicAudio;
    public AudioClip openChest;
    public AudioClip linkDying;
    public AudioClip S_swing;
    public AudioClip S_hit;
    public PlayerController controller;
    public GameObject GetSkil;
    public bool isDied;
    // Start is called before the first frame update
    void Start()
    {
        musicAudio = GetComponent<AudioSource>();
        TurnBackVolume();
    }
    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            if (controller.isDead && !isDied)
            {
                LinkDying();
            }
            else if (GetSkil.activeSelf)
            {
                OpenChest();
            }
        }
    }
    void OpenChest()
    {
        musicAudio.PlayOneShot(openChest, 0.2f);
        musicAudio.volume = 0.3f;
        Invoke("TurnBackVolume", 3);
    }
    void LinkDying()
    {
        musicAudio.PlayOneShot(linkDying, 1);
        musicAudio.volume = 0.3f;
        isDied=true;
    }
   void TurnBackVolume()
    {
        musicAudio.volume = 0.5f;
    }
    public void s_swing()
    {
        musicAudio.PlayOneShot(S_swing, 1);
    }
    public void s_hit()
    {
        musicAudio.PlayOneShot(S_hit, 1);
    }
}
