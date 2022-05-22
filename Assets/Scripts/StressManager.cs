using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// change character animation
public class StressManager : MonoBehaviour
{
    public static StressManager Instance { get; private set; }


    [Tooltip("Stress increment amount.")]
    [SerializeField] int stressBuildupValue = 10;

    [Tooltip("Stress decrement amount.")]
    [SerializeField] int stressReleaseValue = 25;

    [Tooltip("Stress cap amount.")]
    [SerializeField] int stressCap = 100;
    public int StressCap => stressCap;

    [Tooltip("The minimum time in beats player must breathe for to destress.")]
    [Min(0f)]
    [SerializeField] float minBreathTimeInBeats = 0.25f;
    public float MinBreathTimeInBeats => minBreathTimeInBeats;

    [Tooltip("The maximum time in beats player must breathe for to destress.")]
    [Min(0f)]
    [SerializeField] float maxBreathTimeInBeats = 1.25f;
    public float MaxBreathTimeInBeats => maxBreathTimeInBeats;

    private int stressLevel = 0;
    public int StressLevel => stressLevel;


    public static event Action OnGameOver;


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
    }

    private void OnEnable()
    {
        InputSystem.OnNoteDestroyed += AddStress;
    }

    private void OnDisable()
    {
        InputSystem.OnNoteDestroyed -= AddStress;
    }

    public void AddStress(bool isHit) 
    {
        if (!isHit)
        {
            stressLevel = Mathf.Clamp(stressLevel + stressBuildupValue, 0, stressCap);
        }
    }

    public void RemoveStress()
    {
        stressLevel = Mathf.Clamp(stressLevel - stressReleaseValue, 0, stressCap);
    }

    private void Update()
    {
        // Check for gameover
        if (stressLevel == stressCap) 
        {
            CallGameOver();
        }
    }

    public void CallGameOver()
    {
        OnGameOver?.Invoke();
    }

}
