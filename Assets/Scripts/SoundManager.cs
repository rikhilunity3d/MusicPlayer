using UnityEngine;
using Obvious.Soap;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Setup")]
    [SerializeField] private AudioClip[] audioClips; // Optional playlist
    private int currentTrack = 0;
    private AudioSource audioSource;

    [Header("Scriptable Variables")]
    [SerializeField] private BoolVariable isPause;
    [SerializeField] private BoolVariable isMute;
    [SerializeField] private FloatVariable audioCurrentLength;
    [SerializeField] private StringVariable stringAudioCurrentLength;
    [SerializeField] private FloatVariable audioLength;
    [SerializeField] private StringVariable stringAudioLength;
    [SerializeField] private StringVariable songName;
    [SerializeField] private StringVariable songLyricsSync;
    [Header("Lyrics Data")]
    [SerializeField] private AartiLyricsData aartiLyricsData;

    private bool isUpdating = false;
    private int currentLineIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = aartiLyricsData.aartiAudioClip;

        // Subscribe to changes in pause/mute state
        isPause.OnValueChanged += OnPauseValueChanged;
        isMute.OnValueChanged += OnMuteValueChanged;
        audioCurrentLength.OnValueChanged += SyncLyrics;
    }

    private void OnDestroy()
    {
        isPause.OnValueChanged -= OnPauseValueChanged;
        isMute.OnValueChanged -= OnMuteValueChanged;
        audioCurrentLength.OnValueChanged -= SyncLyrics;
        isUpdating = false;
    }

    private void SyncLyrics(float currentTime)
    {
        //songLyricsSync.Value = aartiLyricsData.syncedLyrics[currentLineIndex].line;

        string str = FormatFullTime(currentTime);

        if (currentLineIndex < aartiLyricsData.syncedLyrics.Count && stringAudioCurrentLength.Value.Equals(aartiLyricsData.syncedLyrics[currentLineIndex].time))
        {
            //Debug.Log($"Calling SyncLyrics {currentTime} Current Time in time: {stringAudioCurrentLength.Value} equal {aartiLyricsData.syncedLyrics[currentLineIndex].time}");
            songLyricsSync.Value = aartiLyricsData.syncedLyrics[currentLineIndex].line;
            currentLineIndex++;
        }
    }

    private void OnDisable()
    {
        isUpdating = false;
    }

    // Event handlers for external state change
    private void OnPauseValueChanged(bool isPaused) => Pause(isPaused);
    private void OnMuteValueChanged(bool isMuted) => Mute(isMuted);

    // Start playing the aarti and begin lyric syncing
    private void Play()
    {
        if (audioSource.clip == null)
        {
            Debug.LogWarning("Audio clip not assigned!");
            return;
        }

        audioSource.Play();
        isUpdating = true;

        currentLineIndex = 0;       // Async start lyrics syncing
        StartUpdatingSlider();       // Slider update in parallel
        UpdateSongName();
    }

    // Stop the audio and syncing
    private void Stop()
    {
        isUpdating = false;
        audioSource.Stop();
    }

    // Pause or resume playback
    private void Pause(bool isPaused)
    {
        if (isPaused)
        {
            audioSource.Pause();
            isUpdating = false;
        }
        else
        {
            Play();
        }
    }

    // Mute audio
    private void Mute(bool isMuted) => audioSource.mute = isMuted;

    // Update the synced song name
    private void UpdateSongName()
    {
        songName.Value = aartiLyricsData.aartiTitle;
        Debug.Log("Now Playing: " + aartiLyricsData.aartiTitle);
    }

    // Continuously updates the slider value based on audio time
    private async void StartUpdatingSlider()
    {
        Debug.Log($"audio Source.time: {audioSource.time} | AudioSource Lenght: {audioSource.clip.length}");

        while (isUpdating && audioSource != null && audioSource.isPlaying)
        {
            stringAudioLength.Value = FormatFullTime(audioSource.clip.length);
            audioLength.Value = audioSource.clip.length / 60;
            
            stringAudioCurrentLength.Value = FormatFullTime(audioSource.time);
            audioCurrentLength.Value = audioSource.time / 60;
            await Task.Delay(100); // Update every 0.1s
        }
    }

    // Play the previous track in the list
    public void Previous()
    {
        Stop();

        if (currentTrack == 0)
            currentTrack = audioClips.Length - 1;
        else
            currentTrack--;

        audioSource.clip = audioClips[currentTrack];
        UpdateSongName();
        Pause(isPause); // Resume if not paused
    }

    // Play the next track in the list
    public void Next()
    {
        Stop();

        if (currentTrack == audioClips.Length - 1)
            currentTrack = 0;
        else
            currentTrack++;

        audioSource.clip = audioClips[currentTrack];
        UpdateSongName();
        Pause(isPause); // Resume if not paused
    }

    public string FormatFullTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"hh\:mm\:ss");
    }
}
