using UnityEngine;
using Obvious.Soap;
using System.Threading.Tasks;
using System;

public class SoundManager : MonoBehaviour
{
 //   public Slider slider;
    [SerializeField]
    AudioClip[] audioClips;
    int currentTrack = 0;
    AudioSource audioSource;
    [SerializeField]
    BoolVariable isPause;
    [SerializeField]
    BoolVariable isMute;
    [SerializeField]
    FloatVariable sliderCurrentValue;

    [SerializeField]
    StringVariable songName;

    private bool isUpdating = false;

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
        isPause.OnValueChanged -= OnPauseValueChanged;
        isMute.OnValueChanged -= OnMuteValueChanged;
    }

    private void OnPauseValueChanged(bool isPause) => Pause(isPause);

    private void OnMuteValueChanged(bool isMute) => Mute(isMute);


    void Play()
    {
        audioSource.Play();
        isUpdating = true; // Enable updates when the component starts
        StartUpdatingSlider();

        UpdateSongName();

    }

    private async void StartUpdatingSlider()
    {
        if (!isUpdating || audioSource == null)
            return;
        // Update the slider value
        sliderCurrentValue.Value = Mathf.Clamp(audioSource.time / 100, 0, audioSource.clip.length);

        // Wait for a short interval before updating again
        await Task.Delay(100); // Adjust the delay as needed (in milliseconds)

        // Recursively call the method to continue updates
        StartUpdatingSlider();

        
    }

    private void UpdateSongName()
    {
        songName.Value = audioSource.clip.name;
        Debug.Log("Clip Name"+" "+audioSource.clip.name);
    }

    private void OnDisable()
    {
        isUpdating = false; // Stop updates when the component is disabled
    }

    private void OnDestroy()
    {
        isUpdating = false; // Stop updates when the object is destroyed
    }

    void Pause(bool isPause)
    {
        if (isPause == true)
        {
            audioSource.Pause();
            isUpdating = false; // Stop updates when the object is destroyed            
        }
        else
            Play();
    }

    void Stop()
    {
        isUpdating = false; // Stop updates when the component is disabled
        audioSource.Stop();
    }

    void Mute(bool isMute) => audioSource.mute = isMute;

    public void Previous()
    {
        Stop();

        if (currentTrack == 0)
            currentTrack = audioClips.Length - 1;
        else
            currentTrack--;

        audioSource.clip = audioClips[currentTrack];
        UpdateSongName();

        Pause(isPause);
    }

    public void Next()
    {
        Stop();

        if (currentTrack == audioClips.Length - 1)
            currentTrack = 0;
        else
            currentTrack++;

        audioSource.clip = audioClips[currentTrack];
        UpdateSongName();
        Pause(isPause);
    }

}
