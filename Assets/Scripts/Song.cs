using System.Collections.Generic;
using UnityEngine;


// Store notedata array such as notePosInBeats. 
[CreateAssetMenu(fileName = "NewSong", menuName = "SongData")]
public class Song : ScriptableObject
{

    [Tooltip("The song to be played.")]
    public AudioClip song;
    [Tooltip("The song's beats per minute (bpm).")]
    public int bpm = -1;
    [Tooltip("The note's data for a song's game notes.")]
    public NoteData[] notes; //Song data


    [System.Serializable]
    public class NoteData
    {
        public float notePosInBeats;

        public char keyOfThisNote = ' ';

        [TextArea(0,5)]
        public string displayText;

        public bool showNote;
        public bool isInput;
    }
}
