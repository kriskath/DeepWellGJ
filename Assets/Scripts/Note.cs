using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public GameObject notePrefab;
    public float notePosInBeats;

    //TODO : Key inputs stored here. Maybe array of valid key inputs? Index of valid key determines input mode for player?
    // 3 input settings. {1. Whole Keyboard, 2. 4-key input, 3. 1-key input}
    // NOTE: Space bar is excluded. Must be used for breathing.
}
