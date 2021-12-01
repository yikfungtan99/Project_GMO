using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlayOneShot()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void Play()
    {
        audioSource.Play();
    }
}
