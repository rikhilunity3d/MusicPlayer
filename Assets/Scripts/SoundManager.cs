using UnityEngine;
using UnityEngine.TextCore;
using Obvious.Soap;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] 
    AudioClip[] audioClips;

    int currentTrack = 0;
    AudioSource audioSource;

    [SerializeField]
    BoolVariable isPause;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[0];

        isPause.OnValueChanged += OnPauseValueChanged;

    }

    void OnDestory()
    {
        isPause.OnValueChanged-= OnPauseValueChanged;
    }

    private void OnPauseValueChanged(bool isPause)
    {
        Pause(isPause);
    }

    void Play()
    {
        audioSource.Play();
    }

    void Pause(bool isPause)
    {
        if (isPause == true)
            audioSource.Pause();
        else
            Play();
    }

    void Stop()
    {
        audioSource.Stop();
    }

    void Mute()
    {
        audioSource.mute=true;
    }

    void UnMute()
    {
        audioSource.mute=false;
    }

    public void Previous()
    {
        Stop();

        if(currentTrack == 0)
            currentTrack = audioClips.Length-1;
        else 
            currentTrack--;
        
        audioSource.clip = audioClips[currentTrack];
        
        Pause(isPause);

    }

    public void Next()
    {
        Stop();

        if(currentTrack == audioClips.Length-1)
            currentTrack=0;
        else 
            currentTrack++;
        
        audioSource.clip = audioClips[currentTrack];
        
        Pause(isPause);

    }

}
