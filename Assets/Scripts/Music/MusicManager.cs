using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
using TMPro;
using System.Text;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    [SerializeField]
    private RhythmEventProvider eventProvider;
    [SerializeField]
    private MusicSwitchEvent[] starterSwitchEvents;
    [SerializeField]
    private float startingOffset = 2f;
    [SerializeField]
    private int beatsPerMeasure = 4;
    [SerializeField]
    private TMP_Text debugText;

    public List<MusicSwitchEvent> starterEvents { get; private set; } = new List<MusicSwitchEvent>();
    public MusicWashEvent currentWashEvent { get; private set; }

    private bool isPlaying = false;
    private int numBeats = 0;
    private int measure = 0;

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
        isPlaying = true;
    }

    private void HandleBeat(Beat beat)
    {
        if (isPlaying)
        {
            if (currentWashEvent.IsFinished())
            {
                NextEvent();
            } else
            {
                currentWashEvent.DoEvent(beat);
            }
        }
        ShowDebug(beat);
        IncrementBeats();
    }

    private void IncrementBeats()
    {
        numBeats++;
        measure = numBeats / beatsPerMeasure;
    }

    private void ShowDebug(Beat beat)
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("Measure: " + measure + " Beat:" + (numBeats % beatsPerMeasure + 1));
        text.AppendLine("Total Beats: " + numBeats);
        text.AppendLine("Timestamp: " + (Mathf.Round(beat.timestamp * 100f) / 100f));

        debugText.text = text.ToString();
    }

    private void NextEvent()
    {
        MusicWashEvent nextEvent = currentWashEvent.GetNextWashEvent();

        if (currentWashEvent is MusicSwitchEvent)
        {
            RemoveStarterSwitchEvent((MusicSwitchEvent)currentWashEvent);
        }

        if (nextEvent == null)
        {
            if (starterEvents.Count > 0)
            {
                ChangeEventRandom();
            } else
            {
                EndGame();
            }
        } else
        {
            ChangeEvent(nextEvent);
        }
    }

    public void ChangeEvent(MusicWashEvent nextEvent) 
    {
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
        isPlaying = false;
    }
}
