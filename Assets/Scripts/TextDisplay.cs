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

    public void UpdateText(string textToDisplay, bool isInput) {
        // Update display text
        displayText.text = textToDisplay;

        // Format text
        if (isInput) 
        {
            displayText.color = Color.yellow;
        } 
        else
        {
            displayText.color = Color.white;
        }
    }
}
