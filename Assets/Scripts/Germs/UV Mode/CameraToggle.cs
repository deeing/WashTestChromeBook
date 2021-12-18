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
        isFront = !isFront;

        if (isFront)
        {
            anim.Play("FrontView");
        }
        else
        {
            anim.Play("Default");
        }
        germMap.ToggleFlipped(isFront);
    }
}
