using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InspectionStepButton : MonoBehaviour
{
    [SerializeField]
    private RadialProgress radialProgress;
    [SerializeField]
    private GermType germType;
    [SerializeField]
    private ToggleColor germMapUpToggleColor;
    [SerializeField]
    private ToggleColor germMapDownToggleColor;
    [SerializeField]
    private ToggleColor highlightToggleColor;
    [SerializeField]
    private GameObject highlightBackground;
    [SerializeField]
    private Inspect12Steps inspect12Steps;

    private bool germMapHighlight = false;

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        ToggleGermMapHighlight(false);
    }

    private void Init()
    {
        float percentage = GermManager.instance.GermPercentageByType(germType);

        radialProgress.SetPercent(percentage);

    }

    public void ToggleGermMapHighlight()
    {
        ToggleGermMapHighlight(!germMapHighlight);
    }

    public void ToggleGermMapHighlight(bool status)
    {
        germMapHighlight = status;
        highlightBackground.SetActive(status);
        if (germMapUpToggleColor)
        {
            germMapUpToggleColor.ToggleAuto(germMapHighlight);
        }

        if (germMapDownToggleColor)
        {
            germMapDownToggleColor.ToggleAuto(germMapHighlight);
        }

        highlightToggleColor.ToggleAuto(germMapHighlight);

        if (status)
        {
            inspect12Steps.SetCurrentButton(this);
        }
    
    }
}
