using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionStepButton : MonoBehaviour
{
    [SerializeField]
    private RadialProgress radialProgress;
    [SerializeField]
    private GermType germType;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        float percentage = GermManager.instance.GermPercentageByType(germType);

        radialProgress.SetPercent(percentage);
    }
}
