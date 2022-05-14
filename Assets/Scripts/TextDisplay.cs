using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayText;

    bool canAppend = false;


    void Start()
    {
        // Initialize text as empty
        displayText.text = "";
    }

    public void ClearTextBox()
    {
        displayText.text = "";
        displayText.color = Color.yellow;
        canAppend = true;
    }


    public void UpdateText(string textToDisplay, bool isInput) {
        if (!isInput)
        {
            // Update display text
            displayText.text = textToDisplay;
            displayText.color = Color.white;
            canAppend = false;
        }
    }

    public void AppendToText(string textToAppend)
    {
        if (canAppend)
        {
            displayText.text += textToAppend;
        }
    }
}
