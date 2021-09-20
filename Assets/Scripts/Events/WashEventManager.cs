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
        if (isTransitioning)
        {
            return;
        }

        currentWashEvent.DoEvent();

        if (currentWashEvent.CheckEndEvent())
        {
            StartCoroutine(NextEvent());
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
}
