using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearSwitchOptions : MonoBehaviour
{
    [SerializeField]
    private LeftSlideMenu[] poseOptions;
    [SerializeField]
    private GameObject poseOptionContainer;

    public void TogglePoseOptions(bool status)
    {
        poseOptionContainer.SetActive(status);
        foreach (LeftSlideMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }
    }
}
