using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    [SerializeField]
    private RhythmEventProvider eventProvider;
    [SerializeField]
    private MusicSwitchEvent[] starterSwitchEvents;
    [SerializeField]
    private float startingOffset = 5f;

    public List<MusicSwitchEvent> starterEvents { get; private set; } = new List<MusicSwitchEvent>();
    public MusicWashEvent currentWashEvent { get; private set; }

    private bool hasStarted = false;
    private int numBeats = 0; 

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        SetupRhythm();
        SetupEvents();
        StartCoroutine(StartWashing());
    }

    private void SetupRhythm()
    {
        eventProvider.Register<Beat>(HandleBeat);
    }

    private void SetupEvents()
    {
        starterEvents = new List<MusicSwitchEvent>(starterSwitchEvents);
        currentWashEvent = GetRandomStarter();
    }

    private MusicSwitchEvent GetRandomStarter()
    {
        MusicSwitchEvent randomStarter = starterEvents.RandomElement<MusicSwitchEvent>();
        return randomStarter;
    }

    private IEnumerator StartWashing()
    {
        yield return new WaitForSeconds(startingOffset);
        currentWashEvent.SetupEvent();
        hasStarted = true;
    }

    private void HandleBeat(Beat beat)
    {
        if (hasStarted)
        {
            currentWashEvent.DoEvent(beat);
        }
        numBeats++;
    }

    public void ChangeEvent(MusicWashEvent nextEvent) 
    {
        if (nextEvent == null)
        {
            EndGame();
            return;
        }
        currentWashEvent = nextEvent;
        nextEvent.SetupEvent();
    }

    public void ChangeEventRandom()
    {
        ChangeEvent(GetRandomStarter());
    }

    public void RemoveStarterSwitchEvent(MusicSwitchEvent removeEvent)
    {
        if (starterEvents.Contains(removeEvent))
        {
            starterEvents.Remove(removeEvent);
        }
    }

    private void EndGame()
    {
        Debug.Log("PUT END GAME STUFF HERE");
    }
}
