using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private StressManager stressManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stressManager = GameObject.Find("StressManager").GetComponent<StressManager>();
        Keyboard.current.onTextInput += KeyHit;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("stressLevel", stressManager.StressLevel);
    }

    private void KeyHit(char key)
    {
        // Ignore non-alphabet characters 
        if (!Char.IsLetter(key)) { return; }

        foreach (AnimatorControllerParameter p in animator.parameters)
            if (p.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(p.name);
        animator.SetTrigger(key.ToString());
    }
}
