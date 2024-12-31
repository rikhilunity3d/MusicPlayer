using System;
using Obvious.Soap;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    
    public GameObject PlayButton;
    public GameObject PauseButton;
    public GameObject MuteButton;
    public GameObject UnMuteButton;
    public GameObject NextButton;
    public GameObject PreviousButton;
    public GameObject SoundManager;
    [SerializeField]
    BoolVariable isPause;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayButton.SetActive(true);
        PauseButton.SetActive(false);     
        MuteButton.SetActive(true);
        UnMuteButton.SetActive(false);
        NextButton.SetActive(true);
        PreviousButton.SetActive(true);

        isPause.OnValueChanged += onPlayButtonPressed;
    }

    void OnDestroy()
    {
        isPause.OnValueChanged -= onPlayButtonPressed;
    }
    void onPlayButtonPressed(bool isPause)
    {
        if(isPause == true )
            Pause();
        else
            Play();
    }

    // Update is called once per frame
    

    void Play()
    {
        PlayButton.SetActive(false);
        PauseButton.SetActive(true);
        isPause.Value = false;
        // Play Audio
        //SoundManager.SendMessage("Play");
    }

    void Pause()
    {
        PlayButton.SetActive(true);
        PauseButton.SetActive(false);
        isPause.Value = true;
        // Pause Audio
        //SoundManager.SendMessage("Pause");
    }

    public void Mute()
    {
        // Mute Audio
        MuteButton.SetActive(false);
        UnMuteButton.SetActive(true);     
        //SoundManager.SendMessage("Mute");   
    }

    public void UnMute()
    {
        // UnMute Audio
        MuteButton.SetActive(true);
        UnMuteButton.SetActive(false);

        //SoundManager.SendMessage("UnMute");
    }

}
