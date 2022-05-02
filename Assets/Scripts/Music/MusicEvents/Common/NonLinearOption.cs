using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonLinearOption : MonoBehaviour
{
    [SerializeField]
    private MusicSwitchEvent musicSwitchEvent;
    [SerializeField]
    private bool isLocked = false;
    [SerializeField]
    private Button thisButton;
    [SerializeField]
    private GameObject lockImage;

    private void Awake()
    {
        if (isLocked)
        {
            Lock() ;
        }
    }

    public void ChooseOption()
    {
        MusicManager.instance.SetNonLinearAction(musicSwitchEvent);
        RegisterUsedTwelveSteps();
    }

    public void RegisterUsedTwelveSteps()
    {
        if (HintManager.instance.hasUsedSoap)
        {
            HintManager.instance.hasSeenTwelveStepsHint = true;
            HintManager.instance.ToggleTwelveStepsHint(false);
        }
    }

    private void Lock()
    {
        thisButton.interactable = false;
        lockImage.SetActive(true);
    }
}
