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

    public void TogglePoseOptions(bool status)
    {
        poseOptionContainer.SetActive(status);
        foreach (SlideInMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }

        if (status)
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
            else
            {
                wetButtonColor.ToggleAuto(false);
                soapButtonColor.ToggleAuto(false);
                HintManager.instance.ToggleWetHint(false);
                HintManager.instance.ToggleSoapHint(false);
            }
        }
        else
        {
            HintManager.instance.ToggleWetHint(false);
            HintManager.instance.ToggleSoapHint(false);
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
