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
        
        //TODO : Key inputs stored here. Maybe array of valid key inputs? Index of valid key determines input mode for player?
        // 3 input settings. {1. Whole Keyboard, 2. 4-key input, 3. 1-key input}
        // NOTE: Space bar is excluded. Must be used for breathing.
        public char keyOfThisNote;

        public bool displayText;
    }
}
