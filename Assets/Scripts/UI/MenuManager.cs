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
    private GameObject inspectButton;
    [SerializeField]
    private LeftSlideMenu scrubPowerBarMenu;
    [SerializeField]
    private PercentageBar scrubPowerBar;
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
    [SerializeField]
    private GameObject rhythmStatusMenu;
    [SerializeField]
    private LeftSlideMenu rhythmDebug;
    [SerializeField]
    private SwitchPrompt switchPrompt;
    [SerializeField]
    private MusicResultsMenu musicResultsMenu;
    [SerializeField]
    private AlertMenu scrubAlertMenu;
    [SerializeField]
    private TMP_Text difficultyText;
    [SerializeField]
    private GameObject difficultyMenu;
    [SerializeField]
    private LeftSlideMenu[] poseOptions;
    [SerializeField]
    private LeftSlideMenu finishScrubButton;

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

    public void DisplayPoseOptions(List<MusicSwitchEvent> starterEvents, MusicSwitchEvent currentEvent, int numPoseOptions)
    {
        poseOptionsMenu.DisplayPoseOptions(starterEvents, currentEvent, numPoseOptions);
    }


    public void DisplayPoseOptions(MusicSwitchEvent currentEvent)
    {
        poseOptionsMenu.DisplayPoseOptions(currentEvent);
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

    public void ShowScrubAlert(string message, float duration)
    {
        scrubAlertMenu.Alert(message, duration);
    }

    public void HideScrubAlert()
    {
        scrubAlertMenu.StopAlert();
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
    
    public void IncreaseTotalScore(float amount)
    {
        scoreMenu.IncreaseScore(amount);
    }

    public float GetTotalScore()
    {
        return scoreMenu.GetTotalScore();
    }

    public void ToggleScoreMenu(bool status)
    {
        scoreMenu.gameObject.SetActive(status);
    }

    public void ShowRhythmStatus(RhythmInputStatus status)
    {
        rhythmStatusText.text = status.GetDescription();
    }

    public void ToggleRhythmStatus(bool status)
    {
        rhythmStatusMenu.SetActive(status);
    }

    public void ShowRhythmDebug()
    {
        rhythmDebug.Show();
    }

    public void ToggleRhythmDebug(bool status)
    {
        rhythmDebug.gameObject.SetActive(status);
    }

    public SwitchPrompt GetSwitchPrompt()
    {
        return switchPrompt;
    }

    public MusicResultsMenu GetMusicResultsMenu()
    {
        return musicResultsMenu;
    }

    public void SetDifficultyText(string difficulty)
    {
        difficultyText.text = difficulty;
    }

    public void ToggleDifficultyMenu(bool status)
    {
        difficultyMenu.SetActive(status);
    }

    public void TogglePoseOptions(bool status)
    {
        foreach (LeftSlideMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }
    }

    public void ToggleFinishScrubButton(bool status)
    {
        finishScrubButton.SetVisible(status);
    }
}
