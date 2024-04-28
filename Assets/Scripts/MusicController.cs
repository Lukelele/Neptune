using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource musicSource;
    
    private float _countdown = 0f;
    
    private void FixedUpdate()
    {
        if (_countdown <= 0) PlayRandomClip();
        else _countdown -= Time.fixedUnscaledDeltaTime;
    }
    
    public void PlayRandomClip()
    {
        musicSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        musicSource.Play();
        _countdown = musicSource.clip.length;
    }
}
