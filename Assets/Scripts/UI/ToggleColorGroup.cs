using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleColorGroup : MonoBehaviour
{
    [SerializeField]
    private ToggleColor[] toggleColorGroup;
    [SerializeField]
    private ToggleColor initialOn;

    public void OnEnable()
    {
        if (initialOn)
        {
            ToggleInGroup(initialOn);
        }
    }

    public void OnDisable()
    {
        SetGroup(false);
    }

    public void ToggleInGroup(ToggleColor toggleColor)
    {
        SetGroup(false);
        toggleColor.Toggle(true);
    }

    public void SetGroup(bool status)
    {
        foreach (ToggleColor toggleColor in toggleColorGroup)
        {
            toggleColor.Toggle(status);
        }
    }
}
