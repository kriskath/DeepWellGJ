using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //Debug.Log("Gameover");
        }
        // Check for threshold, update so it's only called the first time
        else if (stressLevel >= 0.5f * stressCap) {
            // Update animations?
            //Debug.Log("Getting pretty stressed!");
        }

        // Diminish over time?
    }
}
