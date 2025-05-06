using UnityEngine;
using TMPro;
using Obvious.Soap;
using System.Threading.Tasks;

public class LyricsManager : MonoBehaviour
{
    public AartiLyricsData lyricsData;
    public TextMeshProUGUI lyricsText;
    public TextMeshProUGUI titleText;
    public AudioSource audioSource;

    [SerializeField]
    BoolVariable isPause;
    [SerializeField]
    BoolVariable isMute;
    [SerializeField]
    private int currentLineIndex = 0;

    void Start()
    {
        isPause.OnValueChanged += OnPauseValueChanged;
        if (lyricsData != null)
        {
            titleText.text = lyricsData.aartiTitle;
            lyricsText.text = ""; // Start empty
            if (lyricsData.aartiAudioClip != null)
            {
                audioSource.clip = lyricsData.aartiAudioClip;
                audioSource.Play();
            }
        }
    }

    private void OnDestroy()
    {
        isPause.OnValueChanged -= OnPauseValueChanged;
    }

    void Update()
    {
        if (lyricsData == null || lyricsData.syncedLyrics == null || lyricsData.syncedLyrics.Count == 0)
            return;

        if (currentLineIndex < lyricsData.syncedLyrics.Count &&
            audioSource.time >= lyricsData.syncedLyrics[currentLineIndex].time)
        {
            lyricsText.text = lyricsData.syncedLyrics[currentLineIndex].line;
            currentLineIndex++;
        }
    }
}
