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
    private GameObject checkList;

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
        checkList.SetActive(!status);
        WashEventManager.instance.isInspectionMode = status;

        if (status)
        {
            currentTutorial = FindActiveTutorial();
            if (currentTutorial)
            {
                currentTutorial.SetActive(false);
            }
        }
        else
        {
            cinemachine.Play("Default");
            if (currentTutorial)
            {
                currentTutorial.SetActive(true);
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
