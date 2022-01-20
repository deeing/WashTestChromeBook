using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GermMap germMap;

    private bool isFront = false;

    public void ToggleCameraView()
    {
        ToggleCameraView(!isFront);
    }

    public void ToggleCameraView(bool isFrontStatus)
    {
        isFront = isFrontStatus;

        if (isFront)
        {
            anim.Play("FrontView");
        }
        else
        {
            anim.Play("Default");
        }
        germMap.ToggleFlipped(isFront);

        if (!HintManager.instance.hasUsedCameraToggle)
        {
            HintManager.instance.hasUsedCameraToggle = true;
            HintManager.instance.ToggleCameraToggleHint(false);

            if (GermManager.instance.HasGerms())
            {
                HintManager.instance.ToggleStillWashHint(true);
            }
            else
            {
                HintManager.instance.ToggleNormalLightHint(true);
            }
        }
    }
}
