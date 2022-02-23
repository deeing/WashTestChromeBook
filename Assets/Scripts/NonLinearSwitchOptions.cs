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
            if (!hintManager.hasUsedWet)
            {
                wetButtonColor.ToggleAuto(true);
                hintManager.ToggleWetHint(true);
            }
            else if (!hintManager.hasUsedSoap)
            {
                soapButtonColor.ToggleAuto(true);
                hintManager.ToggleSoapHint(true);
            }
            else if (!hintManager.hasSeenTwelveStepsHint)
            {
                hintManager.ToggleTwelveStepsHint(true);
                ToggleAllPoseColors(true);
            }
            else if (hintManager.hasFinishedAllGerms && isEasyMode)
            {
                if (!hintManager.hasRinsed)
                {
                    hintManager.ToggleRinseHint(true);
                    rinseButtonColor.ToggleAuto(true);
                }
                else
                {
                    hintManager.ToggleTowelHint(true);
                    towelButtonColor.ToggleAuto(true);
                }
            }
        }
    }

    private void DisableAllRelatedHints()
    {
        hintManager.ToggleRinseHint(false);
        wetButtonColor.ToggleAuto(false);
        soapButtonColor.ToggleAuto(false);
        rinseButtonColor.ToggleAuto(false);
        towelButtonColor.ToggleAuto(false);
        hintManager.ToggleWetHint(false);
        hintManager.ToggleSoapHint(false);
        hintManager.ToggleTwelveStepsHint(false);
        hintManager.ToggleRinseHint(false);
        hintManager.ToggleTowelHint(false);
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
