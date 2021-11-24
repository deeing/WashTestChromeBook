using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwitchEvent : PlayerEvent
{

    public float touchInputwithSensitivity { get; private set; } = 0f;

    public CaligraphyMove caligraphyMove;

   // private int currentMoveIndex = 0;
    private int numConnectionsMade = 0;
    private float animationStep = 0f;
    private float endFrame = 0f;

    private bool completedSwitch = false;
    private bool switchToScrub = false;

    public override void SetupEvent()
    {
        base.SetupEvent();
        completedSwitch = false;
        switchToScrub = false;
        //currentMoveIndex = 0;
        CaligraphyInputManager.instance.ToggleCaligraphy(true);
        CaligraphyInputManager.instance.SetupGuideLines(caligraphyMove);
        CaligraphyInputManager.instance.ToggleInteractable(true);

        animationStep = (caligraphyMove.animationEnd - caligraphyMove.animationStart) / caligraphyMove.symbol.symbolConnections.Count;
    }

    public override void StartEvent()
    {
        base.StartEvent();
        HandAnimations.instance.Reset();
    }

    public override void DoEvent()
    {
        if (WashEventManager.instance.isInspectionMode)
        {
            return;
        }
        if (completedSwitch)
        {
            HandAnimations.instance.PlayAnimationStep(caligraphyMove.animationName, endFrame, Time.deltaTime);
            return;
        }
        if (CaligraphyInputManager.instance.HasDoneCaligraphy(caligraphyMove) && CaligraphyInputManager.instance.UserIsDrawing())
        {
            CompleteSwitch();
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
        //CaligraphyInputManager.instance.ClearGuideLines();
        //CaligraphyInputManager.instance.ToggleInteractable(false);
        //CaligraphyInputManager.instance.ToggleCaligraphy(false);
    }

    public override bool CheckEndEvent()
    {
        return switchToScrub;// CaligraphyInputManager.instance.HasDoneCaligraphy(caligraphyMove) && HandAnimations.instance.HasAnimationReached(endFrame);
    }

    public override void ChangeEvent()
    {
        base.ChangeEvent();
        CaligraphyInputManager.instance.HandleCompleteCaligraphy();
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

    public void CompleteSwitch()
    {
        completedSwitch = true;
        endFrame = caligraphyMove.animationStart + (animationStep * caligraphyMove.symbol.symbolConnections.Count);
        CaligraphyInputManager.instance.SetUserFinishedSymbol(true);
        StartCoroutine(SwitchToScrub());
    }

    private IEnumerator SwitchToScrub()
    {
        MenuManager.instance.ShowAlert("Nice!", .5f);
        CaligraphyInputManager.instance.SetUserFinishedSymbol(false);
        yield return new WaitForSeconds(1f);
        switchToScrub = true;
    }
}
