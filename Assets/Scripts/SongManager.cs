using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Notes:
    // This should be a Singleton.
    // Have the Level Manager determine current song to play. Level Manager tells Song Manager which level it is. Song Manager should determine which song to play
    // Have Music player after a small countdown when level loads.
public class SongManager : MonoBehaviour
{

    public static SongManager Instance { get; private set; }

    [SerializeField]
    private List<Song> songs = new List<Song>();

    [SerializeField]
    [Min(0f)]
    private int notesShownInAdvance = 1;


    private Song currentSong = null;
    private AudioSource audioSource = null;
    private int nextNoteIndex = 0;

    //the current position of the song (in seconds)
    private float songPosInSeconds;

    //the current position of the song (in beats)
    private float songPosInBeats;

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

        currentSong = songs[0];
        //start the song
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //calculate how many seconds is one beat
        secondsPerBeat = 60f / currentSong.bpm;

        //record the time when the song starts
        dspTimeOfSong = (float)AudioSettings.dspTime;

        //start the song
        audioSource.Play();

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
            Instantiate(currentSong.notes[nextNoteIndex].notePrefab);

            //initialize the fields of the music note

            nextNoteIndex++;
        }
    }
}
