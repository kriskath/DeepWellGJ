using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator mouthAnimator;

    private Animator animator;

    //breathe action bound in Input System
    private InputAction breatheAction;

    private bool isAnimActive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Keyboard.current.onTextInput += KeyHit;

        isAnimActive = true; 

        // Get current input map
        InputActionMap inputActions = GameObject.FindObjectOfType<PlayerInput>().currentActionMap;

        // Map OnBreathe to input
        breatheAction = inputActions.FindAction("Breathe");
        breatheAction.started += StartBreatheAnim;
        breatheAction.performed += StopBreatheAnim;

        StressManager.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        Keyboard.current.onTextInput -= KeyHit;
        breatheAction.started -= StartBreatheAnim;
        breatheAction.performed -= StopBreatheAnim;
        StressManager.OnGameOver -= GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("stressLevel", StressManager.Instance.StressLevel);
    }

    private void KeyHit(char key)
    {
        // Ignore non-alphabet characters 
        if (!Char.IsLetter(key) || SongManager.Instance.IsPaused || !isAnimActive) { return; }

        // Ignore if currently breathing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Breathing")) { return; }

        // If panicking, separate sprite
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Concerned"))
        {
            animator.SetTrigger("Talking");
        }
        else 
        {
            mouthAnimator.SetInteger("stressLevel", StressManager.Instance.StressLevel);
            mouthAnimator.SetTrigger("Talking");
        }
    }

    public void StartBreatheAnim(InputAction.CallbackContext context)
    {
        if (SongManager.Instance.IsPaused || !isAnimActive) { return; }

        animator.SetTrigger("BreathStarted");
    }

    public void StopBreatheAnim(InputAction.CallbackContext context)
    {

        animator.SetTrigger("BreathStopped");
    }

    private void GameOver()
    {
        isAnimActive = false;
    }
}
