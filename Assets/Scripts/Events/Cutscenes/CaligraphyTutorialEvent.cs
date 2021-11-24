using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CaligraphyTutorialEvent : CutsceneEvent
{
    [SerializeField]
    private CaligraphySymbol tutorialSymbol;
    [SerializeField]
    private float handMoveSpeed = 1f;
    [SerializeField]
    private float handStartSpeed = 2f;
    [SerializeField]
    private float timeBetweenLoop = 2f;
    [SerializeField]
    private float endOfTutorialPauseTime = 2f;

    [SerializeField]
    private SwitchEvent tutorialEvent;

    private bool finishedTutorial = false;
    private bool isMainTutorialLoop = true;
    private bool startTutorialDrawing = false;
    private Sequence handMoveSequence = null;
    private Vector3 originalHandPosition;
    private WaitForSeconds timeBetweenWait;
    private Coroutine redoCoroutine = null;
    private bool userWasDrawing = false;
    private bool endOfTutorialPause = false;
    private CaligraphyInput caligraphyInput;
    private UILineRenderer lineRenderer;
    private Transform handExample;

    public override void SetupEvent()
    {
        caligraphyInput = CaligraphyInputManager.instance.GetCaligraphyInput();
        lineRenderer = caligraphyInput.GetLineRenderer();
        handExample = CaligraphyInputManager.instance.GetTutorialHand();
        if (tutorialEvent != null)
        {
            tutorialSymbol = tutorialEvent.caligraphyMove.symbol;
            tutorialEvent.SetupEvent();
        } else
        {
            caligraphyInput.SetupGuideLines(tutorialSymbol);
        }
        originalHandPosition = handExample.position;
        timeBetweenWait = new WaitForSeconds(timeBetweenLoop);
        //caligraphyInput.ToggleInteractable(false);
        ToggleAfterTutorialUI(false);
    }

    private void ToggleAfterTutorialUI(bool status)
    {
        MenuManager.instance.ToggleCheckList(status);
        //MenuManager.instance.ToggleSettings(status);
        MenuManager.instance.ToggleInspectButton(status);
    }

    public override bool CheckEndEvent()
    {
        return finishedTutorial;
    }

    public override void DoEvent()
    {
        if (tutorialEvent != null)
        {
            tutorialEvent.DoEvent();
        }

        if (endOfTutorialPause)
        {
            // user has completed and we are just letting them sit and see the completion for a second
            return;
        }

        // check for the end
        if (caligraphyInput.userIsDrawing && CaligraphyInputManager.instance.HasDoneCaligraphy(tutorialSymbol))
        {
            FinishTutorialWithPause();
        }

        // cancel immediately if user is drawing
        if (caligraphyInput.userIsDrawing)
        {
            KillHandMove();
            if (!userWasDrawing)
            {
                CaligraphyInputManager.instance.ClearSymbol();

            }
            userWasDrawing = true;
        } else
        {
            userWasDrawing = false;
            CaligraphyInputManager.instance.ClearSymbol();

            if (startTutorialDrawing)
            {
                caligraphyInput.RemoveUnmarkedPoints();
                lineRenderer.AddPosition(handExample.position);
                lineRenderer.RenderLines();
            } else if (!isMainTutorialLoop)
            {
                Debug.Log("Trying to redo");
                ReDoTutorial();
            }
        }
    }

    private void KillHandMove()
    {
        handExample.DOKill();
        if (handMoveSequence != null)
        {
            handMoveSequence.Kill();
        }
        handExample.gameObject.SetActive(false);
        handExample.position = originalHandPosition;
        isMainTutorialLoop = false;
        startTutorialDrawing = false;
        if (redoCoroutine != null)
        {
            StopCoroutine(redoCoroutine);
            redoCoroutine = null;
        }
    }

    public override void EndEvent()
    {
        ChangeEvent();
    }


    private void SetupHand()
    {
        isMainTutorialLoop = true;
        caligraphyInput.ResetLines();
        int firstButtonId = tutorialSymbol.symbolConnections[0].buttonId1;
        Transform firstButton = caligraphyInput.buttonMap[firstButtonId];
        handExample.gameObject.SetActive(true);

        handExample.DOMove(firstButton.position, handStartSpeed)
            .OnComplete(() => MoveHand());
    }

    public override void StartEvent()
    {
        //MenuManager.instance.ShowAlert("Drag to match the symbol shown", handMoveSpeed);
        SetupHand();
    }

    private void MoveHand()
    {
        startTutorialDrawing = true;

        List<CaligraphyConnection> connections = tutorialSymbol.symbolConnections;
        int firstButtonId = tutorialSymbol.symbolConnections[0].buttonId1;
        Transform firstButton = caligraphyInput.buttonMap[firstButtonId];
        caligraphyInput.AddMarkedPoint(firstButton.position, firstButtonId);

        handMoveSequence = DOTween.Sequence();
        foreach(CaligraphyConnection conn in connections)
        {
            Transform nextButton = caligraphyInput.buttonMap[conn.buttonId2];
            handMoveSequence.Append(handExample.DOMove(nextButton.position, handMoveSpeed)
                .OnComplete(() => MarkPosition(nextButton.position, conn.buttonId2)))
                    .OnComplete(() => ReDoTutorial());
        }
    }

    private void ReDoTutorial()
    {
        isMainTutorialLoop = true;
        startTutorialDrawing = false;
        handExample.gameObject.SetActive(false);
        handExample.position = originalHandPosition;
        if (redoCoroutine == null)
        {
            redoCoroutine = StartCoroutine(QueueTutorial());
        }
    }

    private IEnumerator QueueTutorial()
    {
        yield return timeBetweenWait;
        SetupHand();
        redoCoroutine = null;
    }

    private void MarkPosition(Vector3 position, int buttonId)
    {
        caligraphyInput.AddMarkedPoint(position, buttonId);
    }

    public void FinishTutorialWithPause()
    {
        if (tutorialEvent != null)
        {
            tutorialEvent.CompleteSwitch();
        }

        endOfTutorialPause = true;
        MenuManager.instance.ShowAlert("Perfect! That's how you play! Now keep going!", endOfTutorialPauseTime);
        handExample.gameObject.SetActive(false);
        caligraphyInput.ToggleDrawing(false);
        caligraphyInput.RemoveUnmarkedPoints();
        caligraphyInput.ReRenderLines();
        StartCoroutine(FinishTutorial());
    }

    private IEnumerator FinishTutorial()
    {
        yield return new WaitForSeconds(endOfTutorialPauseTime);
        finishedTutorial = true;
    }

    public override void ChangeEvent()
    {
        CaligraphyInputManager.instance.HandleCompleteCaligraphy();
        KillHandMove();
        ToggleAfterTutorialUI(true);
    }
}
