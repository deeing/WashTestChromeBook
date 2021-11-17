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
    [SerializeField]
    private float coolDownRate = .5f;

    private bool isInspectionMode = false;
    private GameObject currentTutorial;
    private WaitForSeconds coolDownWait;
    private bool isCoolingDown = false;

    private void Awake()
    {
        cinemachine.Play("Intro Cinematic");
        coolDownWait = new WaitForSeconds(coolDownRate);
    }

    public void ToggleInspectionMode()
    {
        if (!isCoolingDown)
        {
            SetInspectionMode(!isInspectionMode);
            isCoolingDown = true;
            StartCoroutine(ResetCooldown());
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return coolDownWait;
        isCoolingDown = false;
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
            //HandAnimations.instance.TransitionPlay("Idle");
            HandAnimations.instance.CrossFade("Idle", .2f);
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
