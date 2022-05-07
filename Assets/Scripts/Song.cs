using UnityEngine;

// Store notedata array such as notePosInBeats. 
[CreateAssetMenu(fileName = "NewSong", menuName = "SongData")]
public class Song : ScriptableObject
{
    public AudioClip song;
    public int bpm = -1;

    public Note[] notes;

    //the index of the next note to be spawned
    int nextIndex = 0;
}
