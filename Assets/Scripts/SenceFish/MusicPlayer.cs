using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicAudio;
    public AudioClip openChest;
    public AudioClip linkDying;
    public PlayerController controller;
    public GameObject GetSkil;
    public bool isDied;
    // Start is called before the first frame update
    void Start()
    {
        musicAudio = GetComponent<AudioSource>();
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
        musicAudio.PlayOneShot(openChest, 1);
        musicAudio.volume = 0.5f;
        Invoke("TurnBackVolume", 3);
    }
    void LinkDying()
    {
        musicAudio.PlayOneShot(linkDying, 1);
        musicAudio.volume = 0.5f;
        isDied=true;
    }
   void TurnBackVolume()
    {
        musicAudio.volume = 1;
    }
}
