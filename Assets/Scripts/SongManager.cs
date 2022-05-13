using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Notes:
    // Have the Level Manager determine current song to play. Level Manager tells Song Manager which level it is. Song Manager should determine which song to play
    // Have Music player after a small countdown when level loads.
    // Have an Event raise when new song plays. 
    // Add a play function to add delay to start.
    // Allow inputs when calling
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
    public float SecondsPerBeat => secondsPerBeat;

    //how much time (in seconds) has passed since the song started. (dsp = digital signal processing)
    private float dspTimeOfSong;

    private Queue<Song.NoteData> textToDisplay = new Queue<Song.NoteData>();

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

        //if need to spawn note
        if (nextNoteIndex < currentSong.notes.Length && 
            currentSong.notes[nextNoteIndex].notePosInBeats < songPosInBeats + notesShownInAdvance)
        {
            //add note to queue if need to spawn text
            if (!string.IsNullOrEmpty(currentSong.notes[nextNoteIndex].displayText)) {
                textToDisplay.Enqueue(currentSong.notes[nextNoteIndex]);
            }

            //show note if need to spawn note
            if (currentSong.notes[nextNoteIndex].showNote) 
            {
                CreateNote(); 
            } 

            nextNoteIndex++;
        }

        //if need to update text
        if (textToDisplay.Count != 0 && 
            textToDisplay.Peek().notePosInBeats <= songPosInBeats) 
        {
            //TEMPORARY: replace with better method
            GameObject.FindObjectOfType<TextDisplay>()?.UpdateText(textToDisplay.Peek().displayText, textToDisplay.Dequeue().isInput);
        }
    }

    private void CreateNote()
    {
        //create note 
        GameNote musicNote = Instantiate(gameNotePrefab, MusicDisplay.Instance.StartPos.position, Quaternion.identity).GetComponent<GameNote>(); 

        //fill note with data. (beatOfThisNote, valid input data)
        musicNote.BeatOfThisNote(currentSong.notes[nextNoteIndex].notePosInBeats);
        musicNote.KeyOfThisNote = currentSong.notes[nextNoteIndex].keyOfThisNote;
        musicNote.IsInput = currentSong.notes[nextNoteIndex].isInput;

        //format if not input
        if (!musicNote.IsInput)
        {
            musicNote.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
    }
}

