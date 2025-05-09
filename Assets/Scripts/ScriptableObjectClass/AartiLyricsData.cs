using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAartiLyrics", menuName = "Aarti/Lyrics Data", order = 1)]
public class AartiLyricsData : ScriptableObject
{
    public string aartiTitle;
    [TextArea(5, 20)]
    public string lyrics;
    public AudioClip aartiAudioClip;

    [System.Serializable]
    public class TimedLine
    {
        public string time="00:00:00"; // in seconds
        public string line;
    }

    public List<TimedLine> syncedLyrics; // synced line-by-line
}
