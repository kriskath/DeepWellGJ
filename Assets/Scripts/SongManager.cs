using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    //event called when note is destroyed. true = note hit, false = note missed.
    public static event Action<bool> OnNoteDestroyed;

    private ContactFilter2D contactFilter;

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

        // Set contact filter properties
        contactFilter.useTriggers = true;
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

        if (nextNoteIndex < currentSong.notes.Length && 
            currentSong.notes[nextNoteIndex].notePosInBeats < songPosInBeats + notesShownInAdvance)
        {
            CreateNote();
            nextNoteIndex++;
        }

        if (Input.anyKeyDown) {
            HitNote();
        }
    }


    private void CreateNote()
    {
        //create note 
        GameNote musicNote = Instantiate(gameNotePrefab, MusicDisplay.Instance.StartPos.position, Quaternion.identity).GetComponent<GameNote>(); 

        //TODO: fill note with data. (beatOfThisNote, valid input data)
        musicNote.BeatOfThisNote(currentSong.notes[nextNoteIndex].notePosInBeats);
    }

    private void HitNote()  
    {
        List<Collider2D> overlapNotes = new List<Collider2D>();

        MusicDisplay.Instance.MusicHitRadius.OverlapCollider(contactFilter, overlapNotes);
        
        // Iterate through overlapping notes
        foreach (Collider2D gameNote in overlapNotes) 
        {
            // Check if correct key hit
            if (Input.GetKeyDown((KeyCode) gameNote.gameObject.GetComponent<GameNote>().KeyOfThisNote)) 
            {
                DestroyNote(gameNote.gameObject, true);
            }
        }
    }

    public void DestroyNote(GameObject gameNote, bool isHit) 
    {
        Destroy(gameNote.gameObject);
        // Invoke event, pass true for hit
        OnNoteDestroyed?.Invoke(isHit);
        // Event subscribers will play sound effects, trigger animation, etc

        if (isHit) 
        {
            Debug.Log("Hit success");
        } 
        else 
        {
            Debug.Log("Hit missed");
        }
    }
}

