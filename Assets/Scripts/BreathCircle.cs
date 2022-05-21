using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathCircle : MonoBehaviour
{
    private Animator animator;
    
    private bool inBreathWindow = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayBreathAnimation(bool play)
    {
        if (play)
        {
            animator.SetTrigger("BreathStarted");
        }
        else 
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Breathing"))
            {
                animator.SetTrigger("BreathStopped");
            }
        }
    }

    public void InBreathWindow(int inWindow)
    {
        inBreathWindow = (inWindow == 0) ? false : true;
    }

    public bool CheckBreathSuccess()
    {
        return inBreathWindow;
    }
}
