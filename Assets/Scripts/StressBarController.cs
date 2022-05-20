using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBarController : MonoBehaviour
{
    private Image stressBar;
    private StressManager stressManager;

    private float stressAmount;

    // Start is called before the first frame update
    private void Awake()
    {
        stressBar = GetComponent<Image>();
        stressManager = GameObject.Find("StressManager").GetComponent<StressManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stressAmount > stressManager.StressLevel + 1 || stressAmount < stressManager.StressLevel - 1)
            stressAmount += 0.5f * (stressManager.StressLevel - stressAmount);
        stressBar.fillAmount = Mathf.Clamp(stressAmount / stressManager.StressCap, 0, 1f);
    }
}
