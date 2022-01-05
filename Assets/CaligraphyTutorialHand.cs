using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CaligraphyTutorialHand : MonoBehaviour
{
    [SerializeField]
    private CaligraphyInput caligraphyInput;
    [SerializeField]
    private float handMoveSpeed = 1f;
    [SerializeField]
    private float handStartSpeed = 2f;
    [SerializeField]
    private float timeBetweenLoop = 2f;
    [SerializeField]
    private Transform tutorialHandImageContainer;

    public bool isDoingTutorial { private set; get; } = false;

    private CaligraphySymbol tutorialSymbol;
    private Sequence handMoveSequence = null;
    private Vector3 originalHandPosition;
    private WaitForSeconds timeBetweenWait;
    private Coroutine redoCoroutine = null;
    private UILineRenderer lineRenderer;

    private void Awake()
    {
        timeBetweenWait = new WaitForSeconds(timeBetweenLoop);
    }

    public void SetupHand(CaligraphySymbol symbol)
    {
        tutorialSymbol = symbol;
        caligraphyInput.ResetLines();
        int firstButtonId = tutorialSymbol.symbolConnections[0].buttonId1;
        Transform firstButton = caligraphyInput.buttonMap[firstButtonId];
        tutorialHandImageContainer.gameObject.SetActive(true);

        tutorialHandImageContainer.DOMove(firstButton.position, handStartSpeed)
            .OnComplete(() => MoveHand());

        lineRenderer = caligraphyInput.GetLineRenderer();

        originalHandPosition = tutorialHandImageContainer.position;
        timeBetweenWait = new WaitForSeconds(timeBetweenLoop);
        isDoingTutorial = true;
    }

    private void MoveHand()
    {
        List<CaligraphyConnection> connections = tutorialSymbol.symbolConnections;
        int firstButtonId = tutorialSymbol.symbolConnections[0].buttonId1;
        Transform firstButton = caligraphyInput.buttonMap[firstButtonId];
        caligraphyInput.AddMarkedPoint(firstButton.position, firstButtonId);

        handMoveSequence = DOTween.Sequence();
        foreach (CaligraphyConnection conn in connections)
        {
            Transform nextButton = caligraphyInput.buttonMap[conn.buttonId2];
            handMoveSequence.Append(tutorialHandImageContainer.DOMove(nextButton.position, handMoveSpeed)
                .OnComplete(() => MarkPosition(nextButton.position, conn.buttonId2)))
                    .OnComplete(() => ReDoTutorial());
        }
    }

    public void KillHandMove()
    {
        CaligraphyInputManager.instance.ClearSymbol();
        tutorialHandImageContainer.DOKill();
        if (handMoveSequence != null)
        {
            handMoveSequence.Kill();
        }
        tutorialHandImageContainer.gameObject.SetActive(false);
        tutorialHandImageContainer.position = originalHandPosition;
        if (redoCoroutine != null)
        {
            StopCoroutine(redoCoroutine);
            redoCoroutine = null;
        }
        isDoingTutorial = false;
    }

    public void ReDoTutorial()
    {
        Debug.Log("trying to redo");    
        tutorialHandImageContainer.gameObject.SetActive(false);
        tutorialHandImageContainer.position = originalHandPosition;
        if (redoCoroutine == null)
        {
            redoCoroutine = StartCoroutine(QueueTutorial());
        }
    }

    private void MarkPosition(Vector3 position, int buttonId)
    {
        caligraphyInput.AddMarkedPoint(position, buttonId);
    }

    private IEnumerator QueueTutorial()
    {
        yield return timeBetweenWait;
        SetupHand(tutorialSymbol);
        redoCoroutine = null;
    }


    public void FinishTutorialWithPause()
    {
        tutorialHandImageContainer.gameObject.SetActive(false);
        caligraphyInput.ToggleDrawing(false);
        caligraphyInput.RemoveUnmarkedPoints();
        caligraphyInput.ReRenderLines();
    }
}
