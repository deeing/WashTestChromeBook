using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : SingletonMonoBehaviour<MenuManager>
{
    [SerializeField]
    private TopDropMenu endMenu;
    [SerializeField]
    private AlertMenu alertMenu;
    [SerializeField]
    private CheckList checkList;
    [SerializeField]
    private GameObject buildPanel;
    [SerializeField]
    private GameObject settingsButton;
    [SerializeField]
    private GameObject inspectButton;
    [SerializeField]
    private LeftSlideMenu scrubPowerBarMenu;
    [SerializeField]
    private PercentageBar scrubPowerBar;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }

    public void ShowEnd(List<string> timeRecordings, string germReport)
    {
        endMenu.gameObject.SetActive(true);
        endMenu.Show();

        endMenu.SetText(BuildEndMenuText(timeRecordings, germReport));
    }

    private string BuildEndMenuText(List<string> timeRecordings, string germReport)
    {
        string endMenuText = "";
        /*foreach (string record in timeRecordings)
        {
            endMenuText += record + "\n";
        }*/
        // just total time for now
        endMenuText += timeRecordings[0] + "\n";

        endMenuText += germReport;
        return endMenuText;
    }

    public void ToggleBuildPanel(bool status)
    {
        buildPanel.SetActive(status);
    }

    public void ToggleSettings(bool status)
    {
        settingsButton.SetActive(status);
    }

    public void ToggleCheckList(bool status)
    {
        checkList.gameObject.SetActive(status);
    }
    public void ToggleInspectButton(bool status)
    {
        inspectButton.SetActive(status);
    }

    public void ShowAlert(string message, float duration)
    {
        alertMenu.Alert(message, duration);
    }

    public void CheckListCheckOffItem()
    {
        checkList.CheckOffItem();
    }

    public void ChecklistHightlightItem(int itemIndex)
    {
        checkList.ChooseItem(itemIndex);
    }

    public void ToggleScrubPowerBar(bool status)
    {
        scrubPowerBarMenu.SetVisible(status);
    }

    public void SetScrubPowerPercentage(float percentage)
    {
        scrubPowerBar.UpdatePercentage(percentage);
    }
}
