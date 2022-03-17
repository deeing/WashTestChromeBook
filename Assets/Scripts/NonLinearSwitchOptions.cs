using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinearSwitchOptions : MonoBehaviour
{
    [SerializeField]
    private SlideInMenu[] poseOptions;
    [SerializeField]
    private GameObject poseOptionContainer;
    [SerializeField]
    private ToggleColor wetButtonColor;
    [SerializeField]
    private ToggleColor soapButtonColor;
    [SerializeField]
    private ToggleColor[] poseOptionsColors;
    [SerializeField]
    private ToggleColor rinseButtonColor;
    [SerializeField]
    private ToggleColor towelButtonColor;

    private HintManager hintManager;
    private bool isEasyMode = false;

    private void Start()
    {
        hintManager = HintManager.instance;

        if (MusicManager.instance.difficulty == LevelDifficulty.Beginner)
        {
            isEasyMode = true;
        }
    }

    public void TogglePoseOptions(bool status)
    {
        poseOptionContainer.SetActive(status);
        foreach (SlideInMenu poseOption in poseOptions)
        {
            poseOption.SetVisible(status);
        }


        if (status)
        {
            HandleHints();
        }
        else
        {
            DisableAllRelatedHints();
        }

    }

    private void HandleHints()
    {
        DisableAllRelatedHints();

        if (HintManagerBase.instance.hintsEnabled)
        { 
            if (!hintManager.hasSeenTwelveStepsHint)
            {
                hintManager.ToggleTwelveStepsHint(true);
                ToggleAllPoseColors(true);
            }
            else if (hintManager.hasSeenTwelveStepsHint && !hintManager.hasUsedInspect)
            {
                hintManager.ToggleInspectHintMenu(true);
            }
        }
    }

    private void DisableAllRelatedHints()
    {
        hintManager.ToggleTwelveStepsHint(false);
        hintManager.ToggleInspectHintMenu(false);
        ToggleAllPoseColors(false);
    }

    private void ToggleAllPoseColors(bool status)
    {
        foreach(ToggleColor poseColor in poseOptionsColors)
        {
            poseColor.ToggleAuto(status);
        }
    }

    public void RegisterUsedWetButton()
    {
        hintManager.hasUsedWet = true;
    }

    public void RegisterUsedSoapButton()
    {
        hintManager.hasUsedSoap = true;
    }

    public void RegisterUsedRinseButton()
    {
        hintManager.hasRinsed = true;
    }


}
