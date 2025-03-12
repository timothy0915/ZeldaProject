using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicAudio;
    // Start is called before the first frame update
    void Start()
    {
        TurnBackVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicAudio.volume <= 4f) Invoke("TurnBackVolume", 3);
    }
   void TurnBackVolume()
    {
        musicAudio.volume = 1;
    }
}
