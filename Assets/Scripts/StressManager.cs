using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add breathing mechanic
// Add thresholds to change character animation
public class StressManager : MonoBehaviour
{
    int stressLevel = 0;

    // stress increment amount when miss note
    [SerializeField] int stressBuildupValue = 10;

    private void OnEnable()
    {
        SongManager.OnNoteDestroyed += AddStress;
    }

    private void OnDisable()
    {
        SongManager.OnNoteDestroyed -= AddStress;
    }

    public void AddStress(bool isHit) {
        if (!isHit)
        {
            stressLevel += stressBuildupValue;

            Debug.Log(stressLevel);
        }
    }

    private void Update()
    {
        // Check for gameover
        // Diminish over time
    }
}
