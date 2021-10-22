using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashEventManager : SingletonMonoBehaviour<WashEventManager>
{
    public VideoManager videoManager;

    [SerializeField]
    private bool playOnStart = true;
    [SerializeField]
    private float timeBetweenEvents = 1f;

    // wash events will be children of this manager each with a single WashEvent component on it
    private List<WashEvent> washEvents = new List<WashEvent>();

    private int currEventIndex = 0;
    [SerializeField]
    private WashEvent currentWashEvent;
    private WashEvent prevWashEvent = null;

    private WaitForSeconds waitBetweenEvents;
    private bool isTransitioning = false;
    private bool finishedEvents = false;
    public bool isInspectionMode = false;

    private float washEventsStartTime = 0f;

    private Dictionary<string, float> timeRecordings = new Dictionary<string, float>();

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }

        SetupWashEvents();

        waitBetweenEvents = new WaitForSeconds(timeBetweenEvents);
    }

    private void Start()
    {
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
            if (washEvent && washEvent.gameObject.activeInHierarchy)
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
        washEventsStartTime = Time.time;
    }

    private void Update()
    {
        if (isTransitioning || finishedEvents || isInspectionMode)
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
        prevWashEvent = currentWashEvent;
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

        float endTime = Time.time;
        AddTimeRecording("Total Time", endTime - washEventsStartTime);
        string germReport = GermManager.instance.GetGermReport();
        MenuManager.instance.ShowEnd(GetAllTimeRecordings(), germReport);
    }

    public void AddTimeRecording(string name, float time)
    {
        timeRecordings.Add(name, time);
    }

    public List<string> GetAllTimeRecordings()
    {
        List<string> timeRecordingsList = new List<string>();

        foreach(KeyValuePair<string, float> timeRecord in timeRecordings)
        {
            string currRecording = timeRecord.Key + " time " + DisplayTime(Mathf.FloorToInt(timeRecord.Value));
            timeRecordingsList.Add(currRecording);
        }

        return timeRecordingsList;
    }

    private string DisplayTime(int totalSeconds)
    {
        string displayString = "";

        // hours
        if (totalSeconds / 60 / 60 > 0)
        {
            string hours = (totalSeconds / 60 / 60).ToString();
            displayString += ZeroPadded(hours) + ":";
        } else if (totalSeconds / 60 > 0)
        {
            string minutes = (totalSeconds / 60).ToString();
            displayString += ZeroPadded(minutes);
        } else
        {
            string seconds = (totalSeconds % 60).ToString();
            displayString += ":" + ZeroPadded(seconds);
        }

        return displayString;
    }

    // zero pads to the left if there is only one digit
    private string ZeroPadded(string number)
    {
        if (number.Length <= 1)
        {
            return "0" + number;
        } else
        {
            return number;
        }
    }

    public WashEvent GetPrevEvent()
    {
        return prevWashEvent;
    }
}
