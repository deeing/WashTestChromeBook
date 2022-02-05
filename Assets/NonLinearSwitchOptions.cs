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

    public void TogglePoseOptions(bool status)
    {
        poseOptionContainer.SetActive(status);
        foreach (SlideInMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }

        if (status && !HintManager.instance.hasUsedWet)
        {
            wetButtonColor.enabled = true;
        } else
        {
            wetButtonColor.enabled = false;
        }
    }
}
