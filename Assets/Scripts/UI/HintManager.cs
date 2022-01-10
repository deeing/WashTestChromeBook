using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : SingletonMonoBehaviour<HintManager>
{
    [SerializeField]
    private TutorialTipMenu inspectTipMenu;

    public bool hasUsedInspect { private set; get; } = false;


    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }
    }

    public void RegisterUsedInspect()
    {
        hasUsedInspect = true;
    }

    public void ToggleInspectTipMenu(bool status)
    {
        inspectTipMenu.ToggleTipMenus(status);
    }

}
