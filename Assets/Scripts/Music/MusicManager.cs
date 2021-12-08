using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
using TMPro;
using System.Text;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    public LevelDifficulty difficulty = LevelDifficulty.Beginner;

    [SerializeField]
    private RhythmEventProvider eventProvider;
    [SerializeField]
    private Transform startEventsContainer;
    [SerializeField]
    private TMP_Text debugText;
    [SerializeField]
    private bool showDebug = true;
    [SerializeField]
    private GameSettings _gameSettings;
    public GameSettings gameSettings { get => _gameSettings; private set => _gameSettings = value; }
    [SerializeField]
    private SongData fallbackSong;

    public List<MusicSwitchEvent> starterEvents { get; private set; } = new List<MusicSwitchEvent>();
    public MusicWashEvent currentWashEvent { get; private set; }

    private bool isPlaying = false;
    private bool isFinished = false;
    private int measure = 0;

    private int currentBeatIndex = 0;
    private Beat currentBeat = null;
    private RhythmData rhythmData;
    private Track<Beat> beatsData;
    private List<Beat> allBeats = new List<Beat>();
    private SongData songData;
    private AudioSource audioSource;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        SetupRhythm();
        SetupEvents();
    }

    private void SetupRhythm()
    {
        if (SongSelection.instance)
        {
            songData = SongSelection.instance.selectedSong;
            difficulty = SongSelection.instance.difficulty;
        }
        else
        {
            songData = fallbackSong;
        }

        MenuManager.instance.SetDifficultyText(difficulty.ToString());
        RhythmPlayer rhythmPlayer = GetComponent<RhythmPlayer>();
        rhythmPlayer.rhythmData = songData.songRhythmData;
        rhythmPlayer.SongEnded += RestartSong;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = songData.songAudio;
        audioSource.Play();
        eventProvider.Register<Beat>(HandleBeat);
        rhythmData = rhythmPlayer.rhythmData;
        beatsData = rhythmData.GetTrack<Beat>();
        beatsData.GetFeatures(allBeats, 0f, rhythmData.audioClip.length);
    }

    private void RestartSong()
    {
        Debug.Log("Replaying song");
        audioSource.Play();
    }

    private void SetupEvents()
    {
        starterEvents = new List<MusicSwitchEvent>();
        foreach (Transform child in startEventsContainer)
        {
            if (child.gameObject.activeSelf)
            {
                MusicSwitchEvent startEvent = child.GetComponent<MusicSwitchEvent>();
                startEvent.RegisterAsStartEvent();
                starterEvents.Add(startEvent);
            }
        }
        currentWashEvent = starterEvents[0];
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
        if (isFinished)
        {
            return;
        }

        if (showDebug)
        {
            ShowDebug(beat);
        }

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
                if (currentWashEvent.ShouldRecord())
                {
                    RecordResults(currentWashEvent);
                }
                NextEvent();
            } else
            {
                currentWashEvent.DoEvent(beat);
            }
        }
        currentBeat = beat;

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
        int safeOffset = Mathf.Min(allBeats.Count - 1, beatIndex + offset);
        return allBeats[safeOffset];
    }

    public Beat GetNextBeat(Beat beat, int offset)
    {
        int beatIndex = beatsData.GetIndex(beat);
        return GetNextBeat(beatIndex, offset);
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
       //text.AppendLine("Measure: " + measure + " Beat:" + (currentBeatIndex % songData.beatsPerMeasure + 1));
        text.AppendLine("Total Beats: " + currentBeatIndex);
        //text.AppendLine("BPM: " + beat.bpm);
        //text.AppendLine("Timestamp: " + (Mathf.Round(beat.timestamp * 100f) / 100f));
        text.AppendLine("Timestamp: " + beat.timestamp);
        //ext.AppendLine("Next Beat: " + (Mathf.Round(GetNextBeat(1).timestamp * 100f) / 100f));

        debugText.text = text.ToString();
    }

    private void NextEvent()
    {
        MusicWashEvent nextEvent = currentWashEvent.GetNextWashEvent();

        /*if (currentWashEvent is MusicSwitchEvent)
        {
            RemoveStarterSwitchEvent((MusicSwitchEvent)currentWashEvent);
        }*/

        if (nextEvent == null)
        {
            /*if (starterEvents.Count > 0)
            {
                ChangeEventRandom();
            } else
            {
                EndGame();
            }*/
            EndGame();
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

    private void RecordResults(MusicWashEvent washEvent)
    {
        MenuManager.instance.GetMusicResultsMenu().AddWashEventResults(washEvent);
    }

    private void EndGame()
    {
        isPlaying = false;
        isFinished = true;
        float totalScore = MenuManager.instance.GetTotalScore();
        MusicResultsMenu musicResultsMenu = MenuManager.instance.GetMusicResultsMenu();
        musicResultsMenu.AddTotalScore(totalScore);
        musicResultsMenu.Show();
    }

    public void OnDestroy()
    {
        eventProvider.Unregister<Beat>(HandleBeat);
    }
}
