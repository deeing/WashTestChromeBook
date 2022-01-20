using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : SingletonMonoBehaviour<HintManager>
{
    [SerializeField]
    private TutorialHintMenu inspectHintMenu;
    [SerializeField]
    private TutorialHintMenu UVHintMenu;
    [SerializeField]
    private TutorialHintMenu cameraToggleHint;
    [SerializeField]
    private TutorialHintMenu stillWashHint;
    [SerializeField]
    private TutorialHintMenu normalLightHint;
    [SerializeField]
    private Transform UVModeHintsContainer;

    public bool hasUsedInspect = false;
    public bool hasUsedCameraToggle = false;


    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }

    public void ToggleInspectHintMenu(bool status)
    {
        inspectHintMenu.ToggleHintMenu(status);
    }

    public void ToggleUVHintMenu(bool status)
    {
        UVHintMenu.ToggleHintMenu(status);
    }

    public void ToggleCameraToggleHint(bool status)
    {
        cameraToggleHint.ToggleHintMenu(status);
    }

    public void ToggleStillWashHint(bool status)
    {
        stillWashHint.ToggleHintMenu(status);
    }

    public void ToggleNormalLightHint(bool status)
    {
        normalLightHint.ToggleHintMenu(status);
    }

    public void DisableAllUVHints()
    {
        foreach(Transform child in UVModeHintsContainer)
        {
            TutorialHintMenu hintMenu = child.GetComponent<TutorialHintMenu>();
            hintMenu.ToggleHintMenu(false);
        }
    }
}
