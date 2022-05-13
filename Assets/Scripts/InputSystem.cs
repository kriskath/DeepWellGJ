using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance { get; private set; }

    //used to detect note hit overlap
    private ContactFilter2D contactFilter;

    //event called when note is destroyed. true = note hit, false = note missed.
    public static event Action<bool> OnNoteDestroyed;

    private InputAction breatheAction;

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

        // Set contact filter properties
        contactFilter.useTriggers = true;

        // Map HitNote to any key press
        Keyboard.current.onTextInput += HitNote;

        // Map Breathe to space bar
        breatheAction = new InputAction("breathe", 
            type: InputActionType.Button,
            binding: "<Keyboard>/space");
        breatheAction.Enable();

        breatheAction.performed += OnBreathe;
    }

    private void HitNote(char keyPressed)  
    {
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

    private void OnBreathe(InputAction.CallbackContext context) {
        // Debug.Log("Breathed for " + context.duration + " seconds");
    }
}
