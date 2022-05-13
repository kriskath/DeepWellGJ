using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayText;

    // Temp: test string
    int indexOfWordDisplayed;
    int lineToDisplay;
    string[] linesToDisplay = {
        "Hey, pal!",
        "Here, in line!"
    };

    // Start is called before the first frame update
    void Start()
    {
        // Initialize text as empty
        displayText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText() {
        // Find end of next word
        indexOfWordDisplayed = linesToDisplay[lineToDisplay].IndexOf(' ', indexOfWordDisplayed +  1);
        
        if (indexOfWordDisplayed != -1)
        {
            // Update display text
            displayText.text = linesToDisplay[lineToDisplay].Substring(0, indexOfWordDisplayed);
        }
        // Reached end of line, display full line
        else {
            displayText.text = linesToDisplay[lineToDisplay];

            // Reset index
            indexOfWordDisplayed = 0;

            // Move to next line
            lineToDisplay++;
        }
    }
}
