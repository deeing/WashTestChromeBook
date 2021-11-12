using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CaligraphyTutorialEvent : CutsceneEvent
{
    [SerializeField]
    private UILineRenderer lineRenderer;
    [SerializeField]
    private UILineRenderer guidelineRenderer;
    [SerializeField]
    private CaligraphySymbol tutorialSymbol;
    [SerializeField]
    private CaligraphyInput caligraphyInput;
    [SerializeField]
    private Transform handExample;
    [SerializeField]
    private float handMoveSpeed = 1f;
    [SerializeField]
    private float handStartSpeed = 2f;
    [SerializeField]
    private float timeBetweenLoop = 2f;

    private bool finishedTutorial = false;
    private bool isMainTutorialLoop = true;
    private bool startTutorialDrawing = false;
    private Sequence handMoveSequence = null;
    private Vector3 originalHandPosition;
    private WaitForSeconds timeBetweenWait;
    private Coroutine redoCoroutine = null;

    public override bool CheckEndEvent()
    {
        return CaligraphyInputManager.instance.HasDoneCaligraphy(tutorialSymbol);
    }

    public override void DoEvent()
    {
        // cancel immediately if user is drawing
        if (caligraphyInput.userIsDrawing)
        {
            KillHandMove();
        }
        else if (startTutorialDrawing)
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
        handExample.gameObject.SetActive(false);
        caligraphyInput.ClearGuideLines();
        caligraphyInput.ResetLines();
        KillHandMove();
        //CaligraphyInputManager.instance.ClearSymbol();
        //caligraphyInput.ToggleInteractable(true);
        MenuManager.instance.ShowAlert("Perfect! That's how you play! Now keep going!", 2f);
    }

    public override void SetupEvent()
    {
        caligraphyInput.SetupGuideLines(tutorialSymbol);
        originalHandPosition = handExample.position;
        timeBetweenWait = new WaitForSeconds(timeBetweenLoop);
        //caligraphyInput.ToggleInteractable(false);
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

    private void FinishTutorial()
    {
        finishedTutorial = true;
    }
}
