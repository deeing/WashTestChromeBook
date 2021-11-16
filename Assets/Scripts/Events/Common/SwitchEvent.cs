using System.Collections;
using UnityEngine;

public abstract class SwitchEvent : PlayerEvent
{

    public float touchInputwithSensitivity { get; private set; } = 0f;

    int currentMoveIndex = 0;

    bool switchToScrub = false;

    public override void SetupEvent()
    {
        base.SetupEvent();
        HandAnimations.instance.Reset();
        switchToScrub = false;
        currentMoveIndex = 0;
        if (caligraphyMoveList.Count > 0)
        {
            CaligraphyInputManager.instance.ToggleCaligraphy(true);
            CaligraphyInputManager.instance.SetupGuideLines(caligraphyMoveList[0]);
            CaligraphyInputManager.instance.ToggleInteractable(true);
        }
    }

    public override void DoEvent()
    {
        if (WashEventManager.instance.isInspectionMode)
        {
            return;
        }

        CaligraphyMove nextMove = caligraphyMoveList[currentMoveIndex];
        if (CaligraphyInputManager.instance.HasDoneCaligraphy(nextMove))
        {
            CaligraphyInputManager.instance.ClearGuideLines();
            CaligraphyInputManager.instance.ToggleInteractable(false);
            HandAnimations.instance.PlayAnimationStep(nextMove.animationName, nextMove.animationStart, nextMove.animationEnd, Time.deltaTime);

            // check if we have finished
            if (HandAnimations.instance.HasAnimationReachedTime(nextMove.animationStart, nextMove.animationEnd))
            {
                CaligraphyInputManager.instance.ClearSymbol();
                HandAnimations.instance.Reset();
                currentMoveIndex++;
                if (currentMoveIndex < caligraphyMoveList.Count)
                {
                    CaligraphyInputManager.instance.ToggleInteractable(true);
                    CaligraphyInputManager.instance.SetupGuideLines(caligraphyMoveList[currentMoveIndex]);
                } else
                {
                    switchToScrub = true;
                }
            }
        }
    }

    public override void EndEvent()
    {
        base.EndEvent();
        CaligraphyInputManager.instance.ToggleCaligraphy(false);
    }
    public override bool CheckEndEvent()
    {
        return switchToScrub;
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
            NeutralIdle();
        }
    }

    public abstract void DoSwitch();
    public virtual float DoTouchInput()
    {
        return 0f;
    }

    private void NeutralIdle()
    {
        HandAnimations.instance.TransitionPlay("Idle", 1f, .2f);
    }

    protected override string GetImpatienceAnimationName()
    {
        return "Neutral Impatience";
    }
}
