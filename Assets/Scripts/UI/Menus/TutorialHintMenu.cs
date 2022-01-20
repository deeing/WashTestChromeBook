using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHintMenu : MonoBehaviour
{
    [SerializeField]
    TutorialHintMenu nextTip;

    private bool hasAlreadyShown = false;

    public void ToggleHintMenu(bool status)
    {
        if (status && !hasAlreadyShown && MenuManager.instance.tipsEnabled)
        {
            gameObject.SetActive(true);
            hasAlreadyShown = true;
        } 
        else
        {
            gameObject.SetActive(false);
        }


    }

    public void PlayNextTip()
    {
        if (nextTip == null)
        {
            return;
        }

        nextTip.ToggleHintMenu(true);
    }

    public void DisableTips()
    {
        ToggleHintMenu(false);
        MenuManager.instance.tipsEnabled = false;
    }
}
