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

    [SerializeField]
    BoolVariable isMute;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[0];

        isPause.OnValueChanged += OnPauseValueChanged;

        isMute.OnValueChanged += OnMuteValueChanged;

    }
    void OnDestory()
    {
        isPause.OnValueChanged-= OnPauseValueChanged;

        isMute.OnValueChanged -= OnMuteValueChanged;
    }

    private void OnPauseValueChanged(bool isPause) => Pause(isPause);

    private void OnMuteValueChanged(bool isMute) => Mute(isMute);
    

    void Play() => audioSource.Play();

    void Pause(bool isPause)
    {
        if (isPause == true)
            audioSource.Pause();
        else
            Play();
    }

    void Stop() => audioSource.Stop();

    void Mute(bool isMute) => audioSource.mute=isMute;
    
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
