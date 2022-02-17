using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearSwitchOptions : MonoBehaviour
{
    [SerializeField]
    private SlideInMenu[] poseOptions;
    [SerializeField]
    private GameObject poseOptionContainer;
    [SerializeField]
    private ToggleColor wetButtonColor;
    [SerializeField]
    private ToggleColor soapButtonColor;
    [SerializeField]
    private ToggleColor[] poseOptionsColors;

    public void TogglePoseOptions(bool status)
    {
        poseOptionContainer.SetActive(status);
        foreach (SlideInMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }

        if (status && HintManagerBase.instance.hintsEnabled)
        {
            if (!HintManager.instance.hasUsedWet)
            {
                wetButtonColor.ToggleAuto(true);
                HintManager.instance.ToggleWetHint(true);
            }
            else if (!HintManager.instance.hasUsedSoap)
            {
                wetButtonColor.ToggleAuto(false);
                soapButtonColor.ToggleAuto(true);
                HintManager.instance.ToggleSoapHint(true);
            }
            else if (!HintManager.instance.hasSeenTwelveStepsHint)
            {
                wetButtonColor.ToggleAuto(false);
                soapButtonColor.ToggleAuto(false);
                HintManager.instance.ToggleWetHint(false);
                HintManager.instance.ToggleSoapHint(false);
                HintManager.instance.ToggleTwelveStepsHint(true);
                ToggleAllPoseColors(true);
            }
            else
            {
                wetButtonColor.ToggleAuto(false);
                soapButtonColor.ToggleAuto(false);
                HintManager.instance.ToggleWetHint(false);
                HintManager.instance.ToggleSoapHint(false);
                HintManager.instance.ToggleTwelveStepsHint(false);
                ToggleAllPoseColors(false);
            }
        }
        else
        {
            HintManager.instance.ToggleWetHint(false);
            HintManager.instance.ToggleSoapHint(false);
        }
    }

    private void ToggleAllPoseColors(bool status)
    {
        foreach(ToggleColor poseColor in poseOptionsColors)
        {
            poseColor.ToggleAuto(status);
        }
    }

    public void RegisterUsedWetButton()
    {
        HintManager.instance.hasUsedWet = true;
    }

    public void RegisterUsedSoapButton()
    {
        HintManager.instance.hasUsedSoap = true;
    }


}
