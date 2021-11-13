using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public abstract class PlayerEvent : WashEvent
{
    [SerializeField]
    private float sensitivity = .005f;
    [SerializeField]
    [Tooltip("Tutorial that should show when this event starts")]
    private GameObject tutorialObject;
    [SerializeField]
    [Tooltip("Whether or not we should time the completion of the current event")]
    private bool shouldTimeEvent = false;
    [SerializeField]
    [Tooltip("Whether or not we should have this as a checklist item")]
    private bool shouldBeChecklistEvent = false;
    [SerializeField]
    protected List<CaligraphyMove> caligraphyMoveList;

    // How long in seconcds it takes for player to become impatient
    protected float impatienceThreshold = 8f;
    // Duration in seconds for the impatience animation to finish and return to idle (not used for normalized animation time)
    protected float impatienceAnimDuration = 3f;
    protected WaitForSeconds impatienceWait;
    // how long user has been idle
    protected float impatienceTimer = 0f;
    protected bool isImpatient = false;

    private float eventStart = 0;

    public override void SetupEvent()
    {
        // make it so not all of the children need an empty function
        impatienceWait = new WaitForSeconds(impatienceAnimDuration);
        if (caligraphyMoveList.Count > 0)
        {
            CaligraphyInputManager.instance.ToggleCaligraphy(true);
            CaligraphyInputManager.instance.SetupGuideLines(caligraphyMoveList[0]);
            CaligraphyInputManager.instance.ToggleInteractable(true);
        }
    }

    public override void StartEvent()
    {
        if (tutorialObject)
        {
            tutorialObject.SetActive(true);
        }

        if (shouldTimeEvent)
        {
            eventStart = Time.time;
        }
    }

    public override void EndEvent()
    {
        EffectsManager.instance.Celebrate();
        if (tutorialObject)
        {
            tutorialObject.SetActive(false);
        }

        if (shouldTimeEvent)
        {
            float eventEnd = Time.time;
            WashEventManager.instance.AddTimeRecording(GetEventType().GetDescription(), eventEnd - eventStart);
        }

        if (shouldBeChecklistEvent)
        {
            MenuManager.instance.CheckListCheckOffItem();
        }
    }

    public override void DoEvent()
    {
        if (WashEventManager.instance.isInspectionMode)
        {
            return;
        }

        CaligraphyMove nextMove = caligraphyMoveList[0];
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
                caligraphyMoveList.RemoveAt(0);
                if (caligraphyMoveList.Count > 0)
                {
                    CaligraphyInputManager.instance.ToggleInteractable(true);
                    CaligraphyInputManager.instance.SetupGuideLines(caligraphyMoveList[0]);
                } 
            } 
        }
    }

    public override bool CheckEndEvent()
    {
        return caligraphyMoveList.Count <= 0;
    }

    public abstract PlayerEventType GetEventType();

    public string GetEventName()
    {
        return GetEventType().GetDescription();
    }

    public bool GetShouldBeChecklistEvent()
    {
        return shouldBeChecklistEvent;
    }

    protected void ResetImpatienceTimer()
    {
        impatienceTimer = 0f;
    }

    protected void IncrementImpatienceTimer(float time)
    {
        impatienceTimer += time;
    }

    protected virtual string GetImpatienceAnimationName()
    {
        return null;
    }

    protected void HandleImpatience()
    {
        HandAnimations.instance.TransitionPlay(GetImpatienceAnimationName(), 1f, .2f);
        isImpatient = true;
        StartCoroutine(ImpatienceEnd());
    }

    public IEnumerator ImpatienceEnd()
    {
        yield return impatienceWait;
        isImpatient = false;
        impatienceTimer = 0f;
    }

    protected bool HasImpatienceAnim()
    {
        return GetImpatienceAnimationName() != null;
    }
}
