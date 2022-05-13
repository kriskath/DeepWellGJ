using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Notes:
    // Have the Level Manager determine current song to play. Level Manager tells Song Manager which level it is. Song Manager should determine which song to play
    // Have Music player after a small countdown when level loads.
    // Have an Event raise when new song plays. 
    // Add a play function to add delay to start.
public class SongManager : MonoBehaviour
{

    public static SongManager Instance { get; private set; }



    [SerializeField]
    [Min(0f)]
    private int notesShownInAdvance = 1;
    public int GetNotesShownInAdvance() => notesShownInAdvance;


    [SerializeField]
    private GameObject gameNotePrefab;
    
    [Space]

    [Tooltip("The list of songs to play")]
    [SerializeField]
    private List<Song> songs = new List<Song>();





    private Song currentSong = null;
    private AudioSource audioSource = null;
    private int nextNoteIndex = 0; //the index of the next note to be played

    //the current position of the song (in seconds)
    private float songPosInSeconds;

    //the current position of the song (in beats)
    private float songPosInBeats;
    public float GetSongPosInBeats() => songPosInBeats;

    //the duration of a beat
    private float secondsPerBeat;

    //how much time (in seconds) has passed since the song started. (dsp = digital signal processing)
    private float dspTimeOfSong;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // TEMPORARY -- Set current song based on level.
        currentSong = songs[0];
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = currentSong.song;
    }

    void Start()
    {
        //calculate how many seconds is one beat
        secondsPerBeat = 60f / currentSong.bpm;

        //record the time when the song starts
        dspTimeOfSong = (float)AudioSettings.dspTime;

        //start the song
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No Audio Source Attached!");
        }
    }

    void Update()
    {
        //calculate the position in seconds
        songPosInSeconds = (float)(AudioSettings.dspTime - dspTimeOfSong);

        //calculate the position in beats
        songPosInBeats = songPosInSeconds / secondsPerBeat;

        //if need to update text
        if (nextNoteIndex < currentSong.notes.Length && 
            Mathf.Abs((float) currentSong.notes[nextNoteIndex].notePosInBeats - songPosInBeats) < 0.01f) 
        {
            Debug.Log("update text");
            if (currentSong.notes[nextNoteIndex].displayText) 
            {
                //TEMPORARY: replace with better method
                GameObject.FindObjectOfType<TextDisplay>()?.UpdateText();
                
                nextNoteIndex++;
            }
        }

        //if need to spawn note
        if (nextNoteIndex < currentSong.notes.Length && 
            currentSong.notes[nextNoteIndex].notePosInBeats < songPosInBeats + notesShownInAdvance)
        {
            if (!currentSong.notes[nextNoteIndex].displayText) 
            {
                CreateNote();
                nextNoteIndex++;
            }
        }
    }

    private void CreateNote()
    {
        //create note 
        GameNote musicNote = Instantiate(gameNotePrefab, MusicDisplay.Instance.StartPos.position, Quaternion.identity).GetComponent<GameNote>(); 

        //TODO: fill note with data. (beatOfThisNote, valid input data)
        musicNote.BeatOfThisNote(currentSong.notes[nextNoteIndex].notePosInBeats);
        musicNote.KeyOfThisNote = currentSong.notes[nextNoteIndex].keyOfThisNote;
    }
}

