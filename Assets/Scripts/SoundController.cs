using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    private int set = 0;

    // Update is called once per frame
    void Start()
    {
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    void Update()
    {
        if (PlayerController.win == true && set == 0) {
            set = 1;
            musicSource.Stop();
            musicSource.loop = false;
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
    }
}
