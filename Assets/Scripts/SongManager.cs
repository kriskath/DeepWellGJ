using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//Notes / TODO:
// Have the Level Manager determine current song to play. Level Manager tells Song Manager which level it is. Song Manager should determine which song to play
// Have Music player after a small countdown when level loads. Add a play function to add delay to start.
// Have an Event raise when new song plays. 
// Allow inputs when in calling state of song (pain)
public class SongManager : MonoBehaviour
{

    public static SongManager Instance { get; private set; }

    [SerializeField]
    [Min(0f)]
    private int notesShownInAdvance = 1;
    public int GetNotesShownInAdvance() => notesShownInAdvance;

    [Tooltip("Time in beats till text box is cleared. After responder sequence and before player input sequence.")]
    [SerializeField]
    float beatsTillClearingTextBox = 0.25f;

    [SerializeField]
    private GameObject gameNotePrefab;

    [Tooltip("Text Display system for the song script.")]
    [SerializeField]
    private TextDisplay textDisplay;
    

    [Space]


    [Tooltip("The list of songs to play")]
    [SerializeField]
    private List<Song> songs = new List<Song>();


    [Space]


    [Header("UI Elements")]

    [Tooltip("Alphabet to display on game notes.")]
    [SerializeField]
    private List<Sprite> alphabet = new List<Sprite>();

    [SerializeField]
    private Sprite playerSpeechBubble;
    [SerializeField]
    private Sprite npcSpeechBubble;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject countdownDisplay;

    [Tooltip("Default sprite display")]
    [SerializeField]
    private Sprite defaultSprite;



    private Song currentSong = null;
    private AudioSource audioSource = null;
    private int nextNoteIndex = 0; //the index of the next note to be displayed

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

    private Animator frogAnimator;

    // change speech bubble tail based on who is talking
    private SpriteRenderer speechRenderer;

    private bool isPaused;
    public bool IsPaused => isPaused;

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

        if (!textDisplay)
        {
            textDisplay = FindObjectOfType<TextDisplay>();
        }

        // TEMPORARY -- Set current song based on level.
        currentSong = songs[0];
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = currentSong.song;

        // Bind pausing music to OnGamePaused
        InputSystem.OnGamePaused += TogglePause;

        speechRenderer = GameObject.Find("MusicDisplay").transform.Find("MusicBar").gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        InputSystem.OnGamePaused -= TogglePause;
    }

    void Start()
    {
        // Find frog mouth animator
        frogAnimator = GameObject.Find("Frog").transform.GetChild(0).GetComponent<Animator>();

        IEnumerator startCoroutine = StartMusicWithDelay(3);
        // Start music
        StartCoroutine(startCoroutine);
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            //calculate the position in seconds
            songPosInSeconds = (float)(AudioSettings.dspTime - dspTimeOfSong);

            //calculate the position in beats
            songPosInBeats = songPosInSeconds / secondsPerBeat;

            CheckToSpawnNote();

            UpdateText();
        }
    }

    private void CheckToSpawnNote()
    {
        //if need to spawn note
        if (nextNoteIndex < currentSong.notes.Length &&
            currentSong.notes[nextNoteIndex].notePosInBeats < songPosInBeats + notesShownInAdvance)
        {
            //add note to queue
            textToDisplay.Enqueue(currentSong.notes[nextNoteIndex]);

            //show note if need to spawn note
            if (currentSong.notes[nextNoteIndex].showNote)
            {
                CreateNote();
            }

            nextNoteIndex++;
        }
    }

    private void UpdateText()
    {
        //if need to update text
        if (textToDisplay.Count != 0 &&
            textToDisplay.Peek().notePosInBeats <= songPosInBeats)
        {
            bool oldCurIsInput = textToDisplay.Peek().isInput;

            // If not input, make frog talk
            if (!oldCurIsInput)
            {
                if (textToDisplay.Peek().showNote)
                {
                    frogAnimator.SetTrigger("TalkOnce");
                }
                else
                {
                    frogAnimator.SetTrigger("TalkMultiple");
                }
            }

            textDisplay.UpdateText(textToDisplay.Peek().displayText, textToDisplay.Dequeue().isInput);

            bool newCurrentIsInput = false;
            if (textToDisplay.Count != 0)
                newCurrentIsInput = textToDisplay.Peek().isInput;

            //transitioning. So allow for appending to text box
            if (!oldCurIsInput && newCurrentIsInput)
            {
                StartCoroutine(CallClearTextBox());
            }

            // change speech bubble tail based on who is talking
            if (oldCurIsInput)
                speechRenderer.sprite = playerSpeechBubble;
            else
                speechRenderer.sprite = npcSpeechBubble;
        }
    }

    IEnumerator CallClearTextBox()
    {
        yield return new WaitForSeconds(secondsPerBeat * beatsTillClearingTextBox);
        textDisplay.ClearTextBox();
    }

    private void CreateNote()
    {
        //create note 
        GameNote musicNote = ObjectPool.Instance.GetPooledObject().GetComponent<GameNote>();
        if (musicNote)
        {
            musicNote.gameObject.transform.position = MusicDisplay.Instance.StartPos.position;
            musicNote.gameObject.transform.rotation = Quaternion.identity;
            musicNote.gameObject.SetActive(true);
        }

        //fill note with data. (beatOfThisNote, valid input data)
        musicNote.BeatOfThisNote(currentSong.notes[nextNoteIndex].notePosInBeats);
        musicNote.KeyOfThisNote = currentSong.notes[nextNoteIndex].keyOfThisNote;
        musicNote.IsInput = currentSong.notes[nextNoteIndex].isInput;

        //format if not input
        if (!musicNote.IsInput)
        {
            //transparency effect
            musicNote.SetSprite(defaultSprite);
            musicNote.SetSpriteColor(new Color(1, 1, 1, 0.5f));
        }
        else
        {
            musicNote.SetSprite(alphabet[musicNote.KeyOfThisNote - 'a']);
            musicNote.SetSpriteColor(new Color(1, 1, 1, 1f));
        }
    }

    private IEnumerator StartMusicWithDelay(int numSeconds)
    {
        // Disable input while counting down
        FindObjectOfType<PlayerInput>().actions.Disable();

        countdownDisplay.SetActive(true);
        for (int i = numSeconds; i > 0; i--)
        {
            countdownDisplay.GetComponent<TMP_Text>().SetText(i.ToString());
            yield return new WaitForSeconds(1f);
        }

        countdownDisplay.SetActive(false);

        // Enable input
        FindObjectOfType<PlayerInput>().actions.Enable();
        // Start music
        PlayMusic();
    }

    private void PlayMusic()
    {
        //calculate how many seconds is one beat
        secondsPerBeat = 60f / currentSong.bpm;

        //record the time when the song starts offset by song timestamp
        dspTimeOfSong = (float)AudioSettings.dspTime - audioSource.time;

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

    private void TogglePause()
    {
        // Toggle between play/pause
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            IEnumerator startCoroutine = StartMusicWithDelay(3);
            // Start music
            StartCoroutine(startCoroutine);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}

