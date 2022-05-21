using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text displayText;

    [SerializeField]
    Color playerTextColor;
    
    [SerializeField]
    Color callerTextColor;


    bool canAppend = false;


    void Start()
    {
        // Initialize text as empty
        displayText.text = "";
    }

    public void ClearTextBox()
    {
        displayText.text = "";
        displayText.color = playerTextColor;
        canAppend = true;
    }


    public void UpdateText(string textToDisplay, bool isInput) {
        if (!isInput)
        {
            // Update display text
            displayText.text = textToDisplay;
            displayText.color = callerTextColor;
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
