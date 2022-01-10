using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTipMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tipMenus;

    private bool hasAlreadyShown = false;

    public void ToggleTipMenus(bool status)
    {
        if (status && !hasAlreadyShown && MenuManager.instance.tipsEnabled)
        {
            SetMenus(true);
            hasAlreadyShown = true;
        } 
        else
        {
            SetMenus(false);
        }
    }

    private void SetMenus(bool status)
    {
        foreach (GameObject tipMenu in tipMenus)
        {
            tipMenu.SetActive(status);
        }
    }

    public void DisableTips()
    {
        ToggleTipMenus(false);
        MenuManager.instance.tipsEnabled = false;
    }
}
