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
    private Transform startEventsContainer;
    [SerializeField]
    private float startingBeatOffset = 2f;
    [SerializeField]
    private int beatsPerMeasure = 4;
    [SerializeField]
    [Tooltip("How many beats a single input move takes")]
    private int beatsPerInputPeriod = 1;
    [SerializeField]
    private TMP_Text debugText;

    public List<MusicSwitchEvent> starterEvents { get; private set; } = new List<MusicSwitchEvent>();
    public MusicWashEvent currentWashEvent { get; private set; }

    private bool isPlaying = false;
    private int measure = 0;

    private int currentBeatIndex = 0;
    private Beat currentBeat = null;
    private RhythmData songData;
    private Track<Beat> beatsData;
    private List<Beat> allBeats = new List<Beat>();

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        SetupRhythm();
        SetupEvents();
        //StartCoroutine(StartWashing());
    }

    private void SetupRhythm()
    {
        eventProvider.Register<Beat>(HandleBeat);
        songData = GetComponent<RhythmPlayer>().rhythmData;
        beatsData = songData.GetTrack<Beat>();
        beatsData.GetFeatures(allBeats, 0f, songData.audioClip.length);
    }

    private void SetupEvents()
    {
        starterEvents = new List<MusicSwitchEvent>();
        foreach (Transform child in startEventsContainer)
        {
            MusicSwitchEvent startEvent = child.GetComponent<MusicSwitchEvent>();
            startEvent.RegisterAsStartEvent();
            starterEvents.Add(startEvent);
        }
        currentWashEvent = GetRandomStarter();
    }

    private MusicSwitchEvent GetRandomStarter()
    {
        MusicSwitchEvent randomStarter = starterEvents.RandomElement<MusicSwitchEvent>();
        return randomStarter;
    }

    private void StartWashing()
    {
        //yield return new WaitForSeconds(startingOffset);
        currentWashEvent.SetupEvent();
        isPlaying = true;
        MenuManager.instance.TogglePreSongMenu(false);
    }

    private void HandleBeat(Beat beat)
    {
        if (!isPlaying)
        {
            if (currentBeatIndex >= startingBeatOffset)
            {
                StartWashing();
            }
        }
        else if (currentWashEvent != null)
        {
            if (currentWashEvent.IsFinished())
            {
                NextEvent();
            } else
            {
                currentWashEvent.DoEvent(beat);
            }
        }
        currentBeat = beat;
        ShowDebug(beat);
        IncrementBeats();
    }

    private void IncrementBeats()
    {
        currentBeatIndex++;
        measure = currentBeatIndex / beatsPerMeasure;
    }

    // Gets the offset number of beats after current beat
    public Beat GetNextBeat(int offset)
    {
        return GetNextBeat(currentBeatIndex, offset);
    }

    // Gets the offset number of beats after the given beat
    public Beat GetNextBeat(int beatIndex, int offset)
    {
        return allBeats[beatIndex + offset];
    }

    public Beat GetNextBeat(Beat beat, int offset)
    {
        int beatIndex = beatsData.GetIndex(beat);
        return allBeats[beatIndex + offset];
    }

    public Beat GetNextBeat(Beat beat)
    {
        return GetNextBeat(beat, beatsPerInputPeriod);
    }

    public int GetBeatsPerInputPeriod()
    {
        return beatsPerInputPeriod;
    }

    public int GetBeatsPerMeasure()
    {
        return beatsPerMeasure;
    }

    private void ShowDebug(Beat beat)
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("Measure: " + measure + " Beat:" + (currentBeatIndex % beatsPerMeasure + 1));
        text.AppendLine("Total Beats: " + currentBeatIndex);
        text.AppendLine("BPM: " + beat.bpm);
        text.AppendLine("Timestamp: " + (Mathf.Round(beat.timestamp * 100f) / 100f));
        text.AppendLine("Next Beat: " + (Mathf.Round(GetNextBeat(1).timestamp * 100f) / 100f));

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

    public bool RemoveStarterSwitchEvent(MusicSwitchEvent removeEvent)
    {
        if (starterEvents.Contains(removeEvent))
        {
            starterEvents.Remove(removeEvent);
            return true;
        }
        return false;
    }

    private void EndGame()
    {
        Debug.Log("PUT END GAME STUFF HERE");
        isPlaying = false;
    }
}
