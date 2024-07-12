using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusicAudio : MonoBehaviour
{
    [SerializeField] public AudioSource Audio;
    [SerializeField] public AudioSource AudioLooped;
    [SerializeField] public float LoopTime;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        Audio.Play();
        AudioLooped.PlayDelayed(GetClipRemainingTime(Audio));
    }

    public static float GetClipRemainingTime(AudioSource source) {
        // Calculate the remainingTime of the given AudioSource,
        // if we keep playing with the same pitch.
        float remainingTime = (source.clip.length - source.time) / source.pitch;
        return remainingTime;
    }
}
