using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ToggleColor : MonoBehaviour
{
    // set by the group it belongs to
    public ToggleColorGroup toggleColorGroup;

    [SerializeField]
    private Color startingColor = Color.white;
    [SerializeField]
    private Color toggledColor = Color.white    ;
    [SerializeField]
    private Image image;
    [SerializeField]
    private float transitionTime = .25f;

    private bool isToggled = false;

    public void OnDisable()
    {
        Toggle(false);
    }

    public void Toggle()
    {
        Toggle(!isToggled);
    }

    public void Toggle(bool status)
    {
        isToggled = status;

        if (status)
        {
            image.CrossFadeColor(toggledColor, transitionTime, false, false);
        } else
        {
            image.CrossFadeColor(startingColor, transitionTime, false, false);
        }
    }

    public void ToggleInGroup()
    {
        if (toggleColorGroup)
        {
            toggleColorGroup.ToggleInGroup(this);
        }
    }
}
