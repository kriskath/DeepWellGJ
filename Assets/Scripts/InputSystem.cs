using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

//TODO:
    // Read in user inputs and display them in text box
public class InputSystem : MonoBehaviour
{
    [Tooltip("Text Display system for the song script.")]
    [SerializeField] TextDisplay textDisplay;

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
        breatheAction.performed += OnBreathe;

        // Map OnPause to input
        pauseAction = inputActions.FindAction("Pause");
        pauseAction.performed += OnPause;
    }

    private void HitNote(char keyPressed)  
    {
        // Ignore breath and pause and while counting down
        if (breatheAction.WasPressedThisFrame() || 
            pauseAction.triggered ||
            !GetComponent<PlayerInput>().actions.enabled) {
            return;
        }

        List<Collider2D> overlapNotes = new List<Collider2D>();

        MusicDisplay.Instance.MusicHitRadius.OverlapCollider(contactFilter, overlapNotes);
        
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

    public void OnBreathe(InputAction.CallbackContext context) 
    {
        // Might want to have a OnSongChanged event so we only need to get secondsperbeat once
        float breatheDurationInBeats = (float) context.duration / SongManager.Instance.SecondsPerBeat;
        Debug.Log("Breathed for " + breatheDurationInBeats + " beats");

        // If breath too short or long
        if (breatheDurationInBeats < StressManager.Instance.MinBreathTimeInBeats || 
            breatheDurationInBeats > StressManager.Instance.MaxBreathTimeInBeats) {
            StressManager.Instance.AddStress(false);
            Debug.Log("Failed breath");
        }
        // If breathe long enough
        else { 
            StressManager.Instance.RemoveStress();
            Debug.Log("Successful breath");
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        OnGamePaused.Invoke();
    }
}
