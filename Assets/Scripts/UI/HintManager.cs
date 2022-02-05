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
    [SerializeField]
    private TutorialHintMenu finishUVHint;

    public bool hasUsedInspect = false;
    public bool hasUsedWet = false;

    private HashSet<string> seenHintSet;


    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        seenHintSet = new HashSet<string>();
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

    public void ToggleFinishUVHint(bool status)
    {
        finishUVHint.ToggleHintMenu(status);
    }

    public void DisableAllUVHints()
    {
        foreach(Transform child in UVModeHintsContainer)
        {
            TutorialHintMenu hintMenu = child.GetComponent<TutorialHintMenu>();
            hintMenu.ToggleHintMenu(false);
        }
    }

    public void SeenHint(string hintId)
    {
        seenHintSet.Add(hintId);
    }

    public bool HasSeenHint(string hintId)
    {
        return seenHintSet.Contains(hintId);
    }
}
