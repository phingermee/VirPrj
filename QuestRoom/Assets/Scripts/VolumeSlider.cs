using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    private AudioSource audioSrc;
    private float musicValue = 0.3f;

    public void SetValue(float val)
    {
        musicValue = val;
    } 

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();    
    }

    void FixedUpdate()
    {
        audioSrc.volume = musicValue;
    }
}
