using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wash.Utilities;

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
    private PoseOptionsMenu poseOptionsMenu;
    [SerializeField]
    private SwitchPrompt switchPromptMenu;
    [SerializeField]
    private TopDropMenu preSong;
    [SerializeField]
    private ScoreMenu scoreMenu;
    [SerializeField]
    private TMP_Text rhythmStatusText;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }

    public void DisplayPoseOptions(List<MusicSwitchEvent> starterEvents, MusicSwitchEvent currentEvent)
    {
        poseOptionsMenu.DisplayPoseOptions(starterEvents, currentEvent);
    }

    public void DisplayPoseOptions(MusicWashEvent nextEvent, MusicSwitchEvent currentEvent)
    {
        poseOptionsMenu.DisplayPoseOptions(nextEvent, currentEvent);
    }

    public void DisplaySwitchPrompt(string promptText, float time)
    {
        switchPromptMenu.ShowPrompt(promptText, time);
    }

    public void HidePoseOptions()
    {
        poseOptionsMenu.Hide();
    }

    public void HideSwitchPrompt()
    {
        switchPromptMenu.HidePrompt();
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
        foreach (string record in timeRecordings)
        {
            endMenuText += record + "\n";
        }

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

    public void ShowAlert(string message, float duration)
    {
        alertMenu.Alert(message, duration);
    }

    public void CheckListCheckOffItem()
    {
        checkList.CheckOffItem();
    }

    public void TogglePreSongMenu(bool status)
    {
        if (status)
        {
            preSong.Show();
        } else
        {
            preSong.Hide();
        }
    }
    
    public void IncreaseScore(float amount)
    {
        scoreMenu.IncreaseScore(amount);
    }

    public void ShowRhythmStatus(RhythmInputStatus status)
    {
        rhythmStatusText.text = status.GetDescription();
    }
}
