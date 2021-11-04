using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wash.Utilities;

public class PoseOption : MonoBehaviour
{
    [SerializeField]
    private TMP_Text poseText;

    private bool isCorrectOption = false;
    private MusicSwitchEvent currentSwitchEvent;
    private SwitchPrompt switchPrompt;

    public void SetupPoseOption(MusicSwitchEvent optionSwitchEvent, MusicSwitchEvent correctSwitchEvent)
    {
        poseText.text = optionSwitchEvent.GetEventType().GetDescription();
        isCorrectOption = optionSwitchEvent.GetEventType() == correctSwitchEvent.GetEventType();
        currentSwitchEvent = correctSwitchEvent;
        switchPrompt = MenuManager.instance.GetSwitchPrompt();
    }

    public void SelectThisPose()
    {
        // pose options will only show up for a switch event

        if (isCorrectOption)
        {
            RhythmInputStatus inputStatus = switchPrompt.GetPromptDistanceStatus();
            currentSwitchEvent.SuccessfulSwitch(inputStatus);
        } else
        {
            currentSwitchEvent.FailedSwitch();
        }
    }
}
