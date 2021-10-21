using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjectHider : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToHide;

    public void ToggleObject(bool status)
    {
        objectToHide.SetActive(status);
    }

    public void ShowObject()
    {
        ToggleObject(true);
    }

    public void HideObject()
    {
        Debug.Log("Hiding");
        ToggleObject(false);
    }
}
