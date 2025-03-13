using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicAudio;
    public AudioClip openChest;
    public AudioClip sinceGame;
    public AudioClip linkDying;
    // Start is called before the first frame update
    void Start()
    {
        TurnBackVolume();
    }
    // Update is called once per frame
    void Update()
    {
        Invoke("TurnBackVolume", 3);
    }
    void OpenChest()
    {
        
    }
    void SinceGame()
    {

    }
    void LinkDying()
    {

    }
   void TurnBackVolume()
    {
        musicAudio.volume = 1;
    }
}
