using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashEventManager : SingletonMonoBehaviour<WashEventManager>
{
    [SerializeField]
    private bool playOnStart = true;
    [SerializeField]
    private float timeBetweenEvents = 1f;

    // wash events will be children of this manager each with a single WashEvent component on it
    private List<WashEvent> washEvents = new List<WashEvent>();

    private int currEventIndex = 0;
    private WashEvent currentWashEvent;

    private WaitForSeconds waitBetweenEvents;
    private bool isTransitioning = false;
    private bool finishedEvents = false;

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }

        SetupWashEvents();

        waitBetweenEvents = new WaitForSeconds(timeBetweenEvents);

        if (playOnStart)
        {
            StartWashEvents();
        }
    }

    private void SetupWashEvents()
    {
        foreach(Transform child in transform)
        {
            WashEvent washEvent = child.GetComponent<WashEvent>();
            if (washEvent)
            {
                washEvents.Add(washEvent);
            }
        }
    }

    private void StartWashEvents()
    {
        currentWashEvent = washEvents[0];
        currentWashEvent.SetupEvent();
        currentWashEvent.StartEvent();
    }

    private void Update()
    {
        if (isTransitioning || finishedEvents)
        {
            return;
        }

        currentWashEvent.DoEvent();

        if (currentWashEvent.CheckEndEvent())
        {
            // has a next event to go to
            if (currEventIndex + 1 < washEvents.Count)
            {
                StartCoroutine(NextEvent());
            }
            else
            {
                currentWashEvent.EndEvent();
                EndScene();
            }
        }
    }

    private IEnumerator NextEvent()
    {
        // end the old
        currentWashEvent.EndEvent();
        isTransitioning = true;

        // transition to the new
        HandAnimations.instance.Reset();
        currEventIndex++;
        currentWashEvent = washEvents[currEventIndex];
        currentWashEvent.SetupEvent();

        yield return waitBetweenEvents;

        currentWashEvent.StartEvent();
        isTransitioning = false;
    }

    private void EndScene()
    {
        HandAnimations.instance.Stop();
        finishedEvents = true;
    }
}
