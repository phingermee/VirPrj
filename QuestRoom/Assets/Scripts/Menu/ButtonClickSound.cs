using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource buttonClick;
    public AudioClip click;

    public void ClickSound()
    {
        buttonClick.PlayOneShot(click);
    }
}
