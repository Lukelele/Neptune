using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The MusicController class is a MonoBehaviour that controls the music in the game.
/// </summary>
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
    
    /// <summary>
    /// PlayRandomClip method plays a random audio clip from the audioClips array.
    /// </summary>
    public void PlayRandomClip()
    {
        musicSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        musicSource.Play();
        _countdown = musicSource.clip.length;
    }
    
    public void PlaySecretSong()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
}
