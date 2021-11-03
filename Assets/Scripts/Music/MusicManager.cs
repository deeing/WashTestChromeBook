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
  //  [SerializeField]
  //  private float startingBeatOffset = 2f;
   // [SerializeField]
  //  private int beatsPerMeasure = 4;
 //   [SerializeField]
  //  [Tooltip("How many beats a single input move takes")]
  //  private int beatsPerInputPeriod = 1;
    [SerializeField]
    private TMP_Text debugText;
    [SerializeField]
    private bool showDebug = true;
    [SerializeField]
    private GameSettings _gameSettings;
    public GameSettings gameSettings { get => _gameSettings; private set => _gameSettings = value; }

    public List<MusicSwitchEvent> starterEvents { get; private set; } = new List<MusicSwitchEvent>();
    public MusicWashEvent currentWashEvent { get; private set; }

    private bool isPlaying = false;
    private int measure = 0;

    private int currentBeatIndex = 0;
    private Beat currentBeat = null;
    private RhythmData rhythmData;
    private Track<Beat> beatsData;
    private List<Beat> allBeats = new List<Beat>();
    private SongData songData;

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
        songData = SongSelection.instance.selectedSong;

        RhythmPlayer rhythmPlayer = GetComponent<RhythmPlayer>();
        rhythmPlayer.rhythmData = songData.songRhythmData;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = songData.songAudio;
        audioSource.Play();
        eventProvider.Register<Beat>(HandleBeat);
        rhythmData = rhythmPlayer.rhythmData;
        beatsData = rhythmData.GetTrack<Beat>();
        beatsData.GetFeatures(allBeats, 0f, rhythmData.audioClip.length);
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
        if (showDebug)
        {
            MenuManager.instance.ShowRhythmDebug();
        }
    }

    private void HandleBeat(Beat beat)
    {
        if (!isPlaying)
        {
            if (currentBeatIndex >= songData.startingBeatOffset)
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
        if (showDebug)
        {
            ShowDebug(beat);
        }
        IncrementBeats();
    }

    private void IncrementBeats()
    {
        currentBeatIndex++;
        measure = currentBeatIndex / songData.beatsPerMeasure;
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
        return GetNextBeat(beat, songData.beatsPerInputPeriod);
    }

    public int GetBeatsPerInputPeriod()
    {
        return songData.beatsPerInputPeriod;
    }

    public int GetBeatsPerMeasure()
    {
        return songData.beatsPerMeasure;
    }

    private void ShowDebug(Beat beat)
    {
        StringBuilder text = new StringBuilder();
        text.AppendLine("Measure: " + measure + " Beat:" + (currentBeatIndex % songData.beatsPerMeasure + 1));
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

        MenuManager.instance.ShowAlert("You did it! (Put end game stuf here)", 5f);
        isPlaying = false;
    }

    public void OnDestroy()
    {
        eventProvider.Unregister<Beat>(HandleBeat);
    }
}
