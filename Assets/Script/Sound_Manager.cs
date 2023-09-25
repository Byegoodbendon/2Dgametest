using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    [SerializeField] private AudioClip jumpAudio, hurtAudio, cherryAudio, diamondAudio, deathAudio;

    private void Awake() 
    {
        instance = this;
    }
    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }
    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }
    public void CherryAudio()
    {
        audioSource.clip = cherryAudio;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }
    public void DiamondAudio()
    {
        audioSource.clip = diamondAudio;
        audioSource.pitch = 1.2f;
        audioSource.Play();
    }
    public void DeathAudio()
    {
        audioSource.clip = deathAudio;
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }
   
}
