using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CheckListItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text checkListText;
    [SerializeField]
    private Toggle checkListToggle;

    private WashEvent checkListEvent;

    public void SetText(string text)
    {
        checkListText.text = text;
    }

    public void SetToggle(bool status)
    {
        checkListToggle.isOn = status;
    }

    public void ChooseItem()
    {
        WashEvent relativeSwitchEvent = WashEventManager.instance.GetSwitchEvent(checkListEvent);
        WashEventManager.instance.ChangeEvent(relativeSwitchEvent);
    }

    public void RegisterEvent(PlayerEvent playerEvent)
    {
        checkListEvent = playerEvent;
    }
}
