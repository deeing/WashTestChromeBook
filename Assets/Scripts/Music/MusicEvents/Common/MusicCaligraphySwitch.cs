using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class MusicCaligraphySwitch : MusicSwitchEvent
{
    [SerializeField]
    private GameObject handSymbolIcon;
    [SerializeField]
    private CaligraphyMove caligraphyMove;
    [SerializeField]
    [Tooltip("Optional other possible move for the same animation")]
    private CaligraphyMove alternateMove;

    private int numConnectionsMade = 0;
    private float animationStep = 0f;
    private float endFrame = 0f;

    private bool completedSwitch = false;
    private bool eventActive = false;
    private float normalGuidelineWait = 3f;

    public override void SetupEvent()
    {
        MenuManager.instance.ShowScrubAlert(GetEventType().GetDescription(), 3f);
        completedSwitch = false;
        CaligraphyInputManager.instance.ToggleCaligraphy(true);
        if (MusicManager.instance.difficulty == LevelDifficulty.Beginner)
        {
            CaligraphyInputManager.instance.SetupGuideLines(caligraphyMove);
        } 
        else if (MusicManager.instance.difficulty == LevelDifficulty.Intermediate)
        {
            StartCoroutine(DelayedGuideLines());
        }
        CaligraphyInputManager.instance.ToggleInteractable(true);
        HandAnimations.instance.Reset();
        eventActive = true;

        if (handSymbolIcon != null)
        {
            handSymbolIcon.SetActive(true);
        }

        animationStep = (caligraphyMove.animationEnd - caligraphyMove.animationStart) / caligraphyMove.symbol.symbolConnections.Count;
    }

    private IEnumerator DelayedGuideLines()
    {
        yield return new WaitForSeconds(normalGuidelineWait);
        CaligraphyInputManager.instance.SetupGuideLines(caligraphyMove);
    }

    public override void DoEvent(Beat beat)
    {
        // do nothing
    }

    private void Update()
    {
        /*
        if (WashEventManager.instance.isInspectionMode)
        {
            return;
        }*/
        if (!eventActive)
        {
            return;
        }
        if (completedSwitch)
        {
            HandAnimations.instance.PlayAnimationStep(caligraphyMove.animationName, endFrame, Time.deltaTime);
            return;
        }
        if (CaligraphyInputManager.instance.UserIsDrawing() &&
            (CaligraphyInputManager.instance.HasDoneCaligraphy(caligraphyMove) || 
                (alternateMove.symbol != null && CaligraphyInputManager.instance.HasDoneCaligraphy(alternateMove))))
        {
            CompleteSwitch();
            return;
        }

        int newNumConnections = CaligraphyInputManager.instance.GetNumValidConnections(caligraphyMove.symbol);
        if (alternateMove.symbol != null)
        {
            // use alternate if it has more valid connections
            int altConnections = CaligraphyInputManager.instance.GetNumValidConnections(alternateMove.symbol);
            newNumConnections = altConnections > newNumConnections ? altConnections : newNumConnections;
        }
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
            //Debug.Log("end " + endFrame);
            HandAnimations.instance.PlayAnimationStep(caligraphyMove.animationName, endFrame, Time.deltaTime);
        }
    }

    public void CompleteSwitch()
    {
        completedSwitch = true;
        endFrame = caligraphyMove.animationStart + (animationStep * caligraphyMove.symbol.symbolConnections.Count);
        StartCoroutine(SwitchToScrub());
    }

    private IEnumerator SwitchToScrub()
    {
        //MenuManager.instance.ShowAlert("Nice!", .5f);

        yield return new WaitForSeconds(1f);
        hasFinished = true;
        CaligraphyInputManager.instance.HandleCompleteCaligraphy();
        CaligraphyInputManager.instance.SetUserFinishedSymbol(false);
        CaligraphyInputManager.instance.ClearSymbol();
        gameObject.SetActive(false);
        eventActive = false;

        if (handSymbolIcon != null)
        {
            handSymbolIcon.SetActive(false);
        }
    }

    private void NeutralIdle()
    {
        //HandAnimations.instance.TransitionPlay("Idle", .05f, .05f);
        HandAnimations.instance.CrossFade("Idle", .05f);
    }
}
