using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add breathing mechanic
// Add thresholds to change character animation
public class StressManager : MonoBehaviour
{
    int stressLevel = 0;

    [Tooltip("Stress increment amount.")]
    [SerializeField] int stressBuildupValue = 10;

    [Tooltip("Stress decrement amount.")]
    [SerializeField] int stressReleaseValue = 10;

    [Tooltip("Stress cap amount.")]
    [SerializeField] int stressCap = 100;

    private void OnEnable()
    {
        SongManager.OnNoteDestroyed += AddStress;
    }

    private void OnDisable()
    {
        SongManager.OnNoteDestroyed -= AddStress;
    }

    public void AddStress(bool isHit) 
    {
        if (!isHit)
        {
            stressLevel = Mathf.Clamp(stressLevel + stressBuildupValue, 0, stressCap);

            Debug.Log(stressLevel);
        }
    }

    public void RemoveStress()
    {
        stressLevel = Mathf.Clamp(stressLevel - stressReleaseValue, 0, stressCap);

        Debug.Log(stressLevel);
    }

    private void Update()
    {
        // Check for gameover
        // Diminish over time
    }
}
