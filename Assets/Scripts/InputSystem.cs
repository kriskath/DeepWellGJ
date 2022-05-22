using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    [Tooltip("Text Display system for the song script.")]
    [SerializeField] TextDisplay textDisplay;

    [Tooltip("Circle effect when breathing.")]
    [SerializeField] BreathCircle breathCircle;

    public static InputSystem Instance { get; private set; }

    //used to detect note hit overlap
    private ContactFilter2D contactFilter;

    //event called when note is destroyed. true = note hit, false = note missed.
    public static event Action<bool> OnNoteDestroyed;

    //event called when game is paused
    public static event Action OnGamePaused;

    //breathe action bound in Input System
    private InputAction breatheAction;

    //pause action bound in Input System
    private InputAction pauseAction;

    // animate hitting of note
    private Animator musicHitAnimator;

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

        // Set contact filter properties
        contactFilter.useTriggers = true;

        // Map HitNote to any key press
        Keyboard.current.onTextInput += HitNote;

        // Get current input map
        InputActionMap inputActions = GetComponent<PlayerInput>().currentActionMap;

        // Map OnBreathe to input
        breatheAction = inputActions.FindAction("Breathe");
        breatheAction.started += OnBreathStarted;
        breatheAction.performed += OnBreathStopped;

        // Map OnPause to input
        pauseAction = inputActions.FindAction("Pause");
        pauseAction.performed += OnPause;

        // get animator component of music hit radius gameobject
        musicHitAnimator = GameObject.Find("MusicDisplay").transform.Find("MusicHitRadius").gameObject.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        breatheAction.started -= OnBreathStarted;
        breatheAction.performed -= OnBreathStopped;
        pauseAction.performed -= OnPause;
        Keyboard.current.onTextInput -= HitNote;
    }

    private void HitNote(char keyPressed)  
    {
        // Ignore breath and pause and while counting down
        if (
            breatheAction.phase == InputActionPhase.Started || 
            pauseAction.triggered || SongManager.Instance.IsPaused || 
            !GetComponent<PlayerInput>().actions.enabled) {
            return;
        }

        Debug.Log(breatheAction.phase);

        List<Collider2D> overlapNotes = new List<Collider2D>();

        MusicDisplay.Instance.MusicHitRadius.OverlapCollider(contactFilter, overlapNotes);

        musicHitAnimator.SetTrigger("hit");
        
        // If list is empty, pressed when no notes
        if (overlapNotes.Count == 0) {
            StressManager.Instance.AddStress(false);
            Debug.Log("Bad timing press");

            return;
        }

        // Iterate through overlapping notes
        foreach (Collider2D gameNote in overlapNotes) 
        {
            textDisplay.AppendToText(keyPressed + "");

            // Check if correct key hit
            if (keyPressed == gameNote.gameObject.GetComponent<GameNote>().KeyOfThisNote)
            {
                DestroyNote(gameNote.gameObject, true);
            }
            else {
                StressManager.Instance.AddStress(false);
                Debug.Log("Incorrect press");
            }
        }
    }

    public void DestroyNote(GameObject gameNote, bool isHit) 
    {
        gameNote.gameObject.SetActive(false);
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

    public void OnBreathStarted(InputAction.CallbackContext context)
    {
        if (SongManager.Instance.IsPaused) { return; }

        breathCircle.PlayBreathAnimation(true);
    }

    public void OnBreathStopped(InputAction.CallbackContext context) 
    {
        if (SongManager.Instance.IsPaused) { return; }

        // Might want to have a OnSongChanged event so we only need to get secondsperbeat once
        float breatheDurationInBeats = (float) context.duration / SongManager.Instance.SecondsPerBeat;
        Debug.Log("Breathed for " + breatheDurationInBeats + " beats");

        // // If breath too short or long
        // if (breatheDurationInBeats < StressManager.Instance.MinBreathTimeInBeats || 
        //     breatheDurationInBeats > StressManager.Instance.MaxBreathTimeInBeats) {
        //     StressManager.Instance.AddStress(false);
        //     Debug.Log("Failed breath");
        // }
        // // If breathe long enough
        // else { 
        //     StressManager.Instance.RemoveStress();
        //     Debug.Log("Successful breath");
        // }

        breathCircle.PlayBreathAnimation(false);
        if (breathCircle.CheckBreathSuccess())
        {
            StressManager.Instance.RemoveStress();
            Debug.Log("Successful breath");
        }
        else
        {
            StressManager.Instance.AddStress(false);
            Debug.Log("Failed breath");
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        OnGamePaused?.Invoke();
    }
}
