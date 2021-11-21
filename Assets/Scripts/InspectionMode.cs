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
    [SerializeField]
    [Tooltip("How often the inspection mode animation should switch from palm up to palm down")]
    private float animationPeriod = 3f;

    private bool isInspectionMode = false;
    private GameObject currentTutorial;
    private WaitForSeconds coolDownWait;
    private WaitForSeconds animPeriodWait;
    private bool isCoolingDown = false;
    private bool isPalmsUp = false;
    private Coroutine animCoroutine = null;

    private void Awake()
    {
        cinemachine.Play("Intro Cinematic");
        coolDownWait = new WaitForSeconds(coolDownRate);
        animPeriodWait = new WaitForSeconds(animationPeriod);
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
            //cinemachine.Play("FrontView");
            currentTutorial = FindActiveTutorial();
            if (currentTutorial)
            {
                currentTutorial.SetActive(false);
            }
            //HandAnimations.instance.TransitionPlay("Idle");
            StartInspectionAnimation();
        }
        else
        {
            cinemachine.Play("Default");
            if (currentTutorial)
            {
                currentTutorial.SetActive(true);
            }
            if (animCoroutine != null)
            {
                StopCoroutine(animCoroutine);
            }
            WashEvent currEvent = WashEventManager.instance.GetCurrentEvent();
            if (currEvent is PlayerEvent)
            {
                ((PlayerEvent)currEvent).ReturnFromInspect();
            }
        }
    }

    private void StartInspectionAnimation()
    {
        HandAnimations.instance.CrossFade("Idle", .2f);

        animCoroutine = StartCoroutine(FlipAnimation());
    }

    private IEnumerator FlipAnimation()
    {
        yield return animPeriodWait;
        ToggleInspectAnimation();
        animCoroutine = StartCoroutine(FlipAnimation());
    }

    private void ToggleInspectAnimation()
    {
        isPalmsUp = !isPalmsUp;

        if (isPalmsUp)
        {
            HandAnimations.instance.CrossFade("Idle Up", .2f);
        } else
        {
            HandAnimations.instance.CrossFade("Idle", .2f);
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
