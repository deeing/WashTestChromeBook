using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionMode : MonoBehaviour
{
    [SerializeField]
    private UVLight uvLight;
    [SerializeField]
    private Transform tutorialCanvas;
    [SerializeField]
    private GameObject cameraButtons;
    [SerializeField]
    private Animator cinemachine;
    [SerializeField]
    private float coolDownRate = .5f;
    [SerializeField]
    private GermMap germMap;
    [SerializeField]
    private ToggleImage toggleImage;
    [SerializeField]
    private CameraToggle cameraToggle;

    [SerializeField]
    private MusicPlayerEvent inspectionEvent;

    private bool isInspectionMode = false;
    private GameObject currentTutorial;
    private WaitForSeconds coolDownWait;
    private bool isCoolingDown = false;
    private MusicWashEvent prevEvent = null;

    private void Awake()
    {
        cinemachine.Play("Intro Cinematic");
        coolDownWait = new WaitForSeconds(coolDownRate);
    }

    public void ToggleInspectionMode()
    {
        if (!isCoolingDown && !MusicManager.instance.isTransitioning)
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

    private void ToggleNonUVMenus(bool status)
    {
        MenuManager.instance.ToggleBuildPanel(!status);
        MenuManager.instance.ToggleSettings(!status);
        MenuManager.instance.ToggleDifficultyMenu(!status);
        MenuManager.instance.ToggleScoreMenu(!status);
        MenuManager.instance.ToggleRhythmDebug(!status);
        MenuManager.instance.ToggleRhythmStatus(!status);
        HintManager.instance.ToggleInspectHintMenu(false);
    }

    public void SetInspectionMode(bool status)
    {
        isInspectionMode = status;
        uvLight.SetUvMode(status);
        cameraButtons.SetActive(status);
        ToggleNonUVMenus(status);
        germMap.ToggleMap(status);
        toggleImage.ShowToggleSprite(status);

        if (status)
        {
            //cinemachine.Play("FrontView");
            currentTutorial = FindActiveTutorial();
            if (currentTutorial)
            {
                currentTutorial.SetActive(false);
            }
            //HandAnimations.instance.TransitionPlay("Idle");
            prevEvent = MusicManager.instance.GetCurrentEvent();
            MusicManager.instance.HardSwitchEvent(inspectionEvent);
            if (!HintManager.instance.hasUsedInspect)
            {
                HintManager.instance.ToggleUVHintMenu(true);
                HintManager.instance.hasUsedInspect = true;
            }
        }
        else
        {
            cinemachine.Play("Default");
            if (currentTutorial)
            {
                currentTutorial.SetActive(true);
            }
            MusicManager.instance.HardSwitchEvent(prevEvent);
            HintManager.instance.DisableAllUVHints();
            cameraToggle.ToggleCameraView(false);
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
