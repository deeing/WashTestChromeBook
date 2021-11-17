using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionMode : MonoBehaviour
{
    [SerializeField]
    private GameObject uvLightButton;
    [SerializeField]
    private Transform tutorialCanvas;
    [SerializeField]
    private GameObject cameraButtons;
    [SerializeField]
    private Animator cinemachine;

    private bool isInspectionMode = false;
    private GameObject currentTutorial;

    private void Awake()
    {
        cinemachine.Play("Intro Cinematic");
    }

    public void ToggleInspectionMode()
    {
        SetInspectionMode(!isInspectionMode);
    }

    public void SetInspectionMode(bool status)
    {
        isInspectionMode = status;
        uvLightButton.SetActive(status);
        cameraButtons.SetActive(status);
        WashEventManager.instance.isInspectionMode = status;
        MenuManager.instance.ToggleCheckList(!status);
        MenuManager.instance.ToggleBuildPanel(!status);
        MenuManager.instance.ToggleSettings(!status);
        HandAnimations.instance.Reset();

        if (CaligraphyInputManager.instance.CurrentEventIsCaligraphy())
        {
            CaligraphyInputManager.instance.ToggleCaligraphy(!status);
        }

        if (status)
        {
            cinemachine.Play("FrontView");
            currentTutorial = FindActiveTutorial();
            if (currentTutorial)
            {
                currentTutorial.SetActive(false);
            }
            HandAnimations.instance.TransitionPlay("Idle" );
        }
        else
        {
            cinemachine.Play("Default");
            if (currentTutorial)
            {
                currentTutorial.SetActive(true);
            }
            WashEvent currEvent = WashEventManager.instance.GetCurrentEvent();
            if (currEvent is PlayerEvent)
            {
                ((PlayerEvent)currEvent).ReturnFromInspect();
            }
        }
    }

    private GameObject FindActiveTutorial()
    {
        foreach (Transform child in tutorialCanvas)
        {
            GameObject tutorialObj = child.gameObject;
            if (tutorialObj.activeInHierarchy)
            {
                return tutorialObj;
            }
        }
        return null;
    }
}
