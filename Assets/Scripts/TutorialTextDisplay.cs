using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialTextDisplay : MonoBehaviour
{
    [SerializeField]
    [TextArea()]
    private List<string> textToDisplay = new List<string>();

    [SerializeField]
    private Animator[] animators;

    [SerializeField]
    TMP_Text displayText;

    private int currentTextToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize text displayed
        currentTextToDisplay = 0;
        displayText.text = textToDisplay[currentTextToDisplay];
    }

    public void UpdateLine(int changeInLines)
    {
        // Increment or decrement line
        currentTextToDisplay += changeInLines;
        // Clamp to allowed values
        currentTextToDisplay = Mathf.Clamp(currentTextToDisplay, 0, textToDisplay.Count - 1);
        // Update text displayed
        displayText.text = textToDisplay[currentTextToDisplay];
        // Update animation displayed
        foreach (Animator animator in animators)
        {
            animator.SetInteger("currentTextToDisplay", currentTextToDisplay);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
