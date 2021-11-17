using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchEvent : PlayerEvent
{

    public float touchInputwithSensitivity { get; private set; } = 0f;

    public CaligraphyMove caligraphyMove;

    private int currentMoveIndex = 0;
    private int numConnectionsMade = 0;
    private float animationStep = 0f;
    private float endFrame = 0f;

    private bool switchToScrub = false;

    public override void SetupEvent()
    {
        base.SetupEvent();
        HandAnimations.instance.Reset();
        switchToScrub = false;
        currentMoveIndex = 0;
        CaligraphyInputManager.instance.ToggleCaligraphy(true);
        CaligraphyInputManager.instance.SetupGuideLines(caligraphyMove);
        CaligraphyInputManager.instance.ToggleInteractable(true);

        animationStep = (caligraphyMove.animationEnd - caligraphyMove.animationStart) / caligraphyMove.symbol.symbolConnections.Count;
    }

    public override void DoEvent()
    {
        if (WashEventManager.instance.isInspectionMode)
        {
            return;
        }
        int newNumConnections = CaligraphyInputManager.instance.GetNumValidConnections(caligraphyMove.symbol);
        if (newNumConnections > numConnectionsMade)
        {
            numConnectionsMade = newNumConnections;
            //float startOfKey = caligraphyMove.animationStart + (animationStep * numConnectionsMade - 1);
            endFrame = caligraphyMove.animationStart + (animationStep * numConnectionsMade);
        } 
        else if (newNumConnections == 0)
        {
            numConnectionsMade = 0;
            HandAnimations.instance.Reset();
            NeutralIdle();
        }
        else if (newNumConnections == numConnectionsMade)
        {
            HandAnimations.instance.PlayAnimationStep(caligraphyMove.animationName, endFrame, Time.deltaTime);
        }
    }

    public override void EndEvent()
    {
        base.EndEvent();
        CaligraphyInputManager.instance.ClearGuideLines();
        CaligraphyInputManager.instance.ToggleInteractable(false);
        CaligraphyInputManager.instance.ToggleCaligraphy(false);
        switchToScrub = true;
    }
    public override bool CheckEndEvent()
    {
        return CaligraphyInputManager.instance.HasDoneCaligraphy(caligraphyMove);
    }

    public override void ChangeEvent()
    {
        base.ChangeEvent();
        CaligraphyInputManager.instance.ClearGuideLines();
        CaligraphyInputManager.instance.ClearSymbol();
        CaligraphyInputManager.instance.ToggleInteractable(false);
        HandAnimations.instance.Reset();
        CaligraphyInputManager.instance.ToggleCaligraphy(false);

        if (!switchToScrub)
        {
            //NeutralIdle();
        }
    }

    public abstract void DoSwitch();
    public virtual float DoTouchInput()
    {
        return 0f;
    }

    private void NeutralIdle()
    {
        //HandAnimations.instance.TransitionPlay("Idle", .05f, .05f);
        HandAnimations.instance.CrossFade("Idle", .05f);
    }

    protected override string GetImpatienceAnimationName()
    {
        return "Neutral Impatience";
    }

    public override void ReturnFromInspect()
    {
        CaligraphyInputManager.instance.ClearGuideLines();
        CaligraphyInputManager.instance.ClearSymbol();
        SetupEvent();
    }
}
