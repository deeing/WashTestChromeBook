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
    private WashEvent relativeSwitchEvent;
    private int index;

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
        WashEventManager.instance.ChangeEvent(relativeSwitchEvent);
        MenuManager.instance.ChecklistHightlightItem(index);
    }

    public void RegisterEvent(PlayerEvent playerEvent, int index)
    {
        this.index = index;
        checkListEvent = playerEvent;
        relativeSwitchEvent = WashEventManager.instance.GetSwitchEvent(checkListEvent);
    }
}
