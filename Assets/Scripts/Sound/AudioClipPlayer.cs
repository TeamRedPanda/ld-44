using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioClipPlayer : MonoBehaviour
{
    public AudioSource AudioSource;

    public void Play(AudioClip clip)
    {
        AudioSource.clip = clip;
        AudioSource.Play();
        Destroy(this.gameObject, clip.length);
    }
}
