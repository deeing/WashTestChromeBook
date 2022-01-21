using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHintMenu : MonoBehaviour
{
    [SerializeField]
    private TutorialHintMenu nextTip;
    [SerializeField]
    private string seenHintId;

    private bool hasAlreadyShown = false;

    public void ToggleHintMenu(bool status)
    {
        if (status && !hasAlreadyShown && MenuManager.instance.tipsEnabled)
        {
            gameObject.SetActive(true);
            hasAlreadyShown = true;

            // register the hint as seen
            if (seenHintId != null && seenHintId.Length > 0)
            {
                HintManager.instance.SeenHint(seenHintId);
            }
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
