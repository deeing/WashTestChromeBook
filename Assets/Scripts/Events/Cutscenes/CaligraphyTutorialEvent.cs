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
    private float handMoveSpeed = 3f;

    private bool finishedTutorial = false;
    private bool startDrawing = false;

    public override bool CheckEndEvent()
    {
        return finishedTutorial;
    }

    public override void DoEvent()
    {
        if (startDrawing)
        {
            caligraphyInput.RemoveUnmarkedPoints();
            lineRenderer.AddPosition(handExample.position);
            lineRenderer.RenderLines();
        }
    }

    public override void EndEvent()
    {
        handExample.gameObject.SetActive(false);
        caligraphyInput.ClearGuideLines();
        caligraphyInput.ResetLines();
        caligraphyInput.ToggleInteractable(true);
        MenuManager.instance.ShowAlert("Now you try!", 2f);
    }

    public override void SetupEvent()
    {
        caligraphyInput.SetupGuideLines(tutorialSymbol);

        caligraphyInput.ToggleInteractable(false);
    }

    private void SetupHand()
    {
        int firstButtonId = tutorialSymbol.symbolConnections[0].buttonId1;
        Transform firstButton = caligraphyInput.buttonMap[firstButtonId];
        handExample.gameObject.SetActive(true);

        handExample.DOMove(firstButton.position, 5f)
            .OnComplete(() => MoveHand());
    }

    public override void StartEvent()
    {
        MenuManager.instance.ShowAlert("Drag to match the symbol shown", handMoveSpeed);
        SetupHand();
    }

    private void MoveHand()
    {
        startDrawing = true;

        List<CaligraphyConnection> connections = tutorialSymbol.symbolConnections;
        int firstButtonId = tutorialSymbol.symbolConnections[0].buttonId1;
        Transform firstButton = caligraphyInput.buttonMap[firstButtonId];
        caligraphyInput.AddMarkedPoint(firstButton.position, firstButtonId);

        Sequence moveSequence = DOTween.Sequence();
        foreach(CaligraphyConnection conn in connections)
        {
            Transform nextButton = caligraphyInput.buttonMap[conn.buttonId2];
            moveSequence.Append(handExample.DOMove(nextButton.position, handMoveSpeed)
                .OnComplete(() => MarkPosition(nextButton.position, conn.buttonId2)))
                    .OnComplete(() => FinishTutorial());
        }
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
