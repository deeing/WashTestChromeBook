using RhythmTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wash.Utilities;
using TMPro;
using System.Text;
using System;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    public LevelDifficulty difficulty = LevelDifficulty.Beginner;
    public bool nonLinearMode = false;


    [SerializeField]
    private RhythmEventProvider eventProvider;
    [SerializeField]
    private Transform startEventsContainer;
    [SerializeField]
    private Transform scrubEventsContainer;
    [SerializeField]
    private TMP_Text debugText;
    [SerializeField]
    private bool showDebug = true;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;

    public bool isTransitioning { get; private set; } = true;

    [SerializeField]
    private GameSettings _gameSettings;
    public GameSettings gameSettings { get => _gameSettings; private set => _gameSettings = value; }
    [SerializeField]
    private SongData fallbackSong;
    [SerializeField]
    private NonLinearCalSwitch _nonLinearCalSwitch;
    public NonLinearCalSwitch nonLinearCalSwitch { get => _nonLinearCalSwitch; private set => _nonLinearCalSwitch = value; }
    [SerializeField]
    private NonLinearAnimationSwitch[] nonScrubPointEvents;

    public List<MusicCaligraphySwitch> starterEvents { get; private set; } = new List<MusicCaligraphySwitch>();
    public MusicWashEvent currentWashEvent { get; private set; }
    public MusicScrubEvent fireDoubleScrubEvent { get; private set; }

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
    private DateTime songStart;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        SetupRhythm();
        SetupEvents();
        SetupSkin();
    }

    private void SetupRhythm()
    {
        if (SongSelection.instance)
        {
            songData = SongSelection.instance.selectedSong;
            difficulty = SongSelection.instance.difficulty;
            nonLinearMode = SongSelection.instance.nonLinear;   
        }
        else
        {
            songData = fallbackSong;
        }

        MenuManager.instance.SetDifficultyText(difficulty.GetDescription());
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
        songStart = DateTime.Now;
    }

    private void RestartSong()
    {
        Debug.Log("Replaying song");
        audioSource.Play();
    }

    private void SetupEvents()
    {
        starterEvents = new List<MusicCaligraphySwitch>();
        foreach (Transform child in startEventsContainer)
        {
            if (child.gameObject.activeSelf)
            {
                MusicCaligraphySwitch startEvent = child.GetComponent<MusicCaligraphySwitch>();
                starterEvents.Add(startEvent);
            }
        }

        if (nonLinearMode)
        {
            currentWashEvent = nonLinearCalSwitch;
        }
        else
        {
            currentWashEvent = starterEvents[0];
        }

        SetupFireDouble();
    }

    private void SetupFireDouble()
    {
        List<MusicScrubEvent> scrubEvents = GetScrubEvents();
        fireDoubleScrubEvent = scrubEvents.RandomElement();

        //MenuManager.instance.ShowAlert("Fire:" + fireDoubleScrubEvent.name, 5f);
    }

    private void SetupSkin()
    {
        SkinMapping savedSkin = gameSettings.GetChosenSkin();
        SetSkin(savedSkin);
    }

    /*private MusicSwitchEvent GetRandomStarter()
    {
        MusicSwitchEvent randomStarter = starterEvents.RandomElement<MusicSwitchEvent>();
        return randomStarter;
    }*/

    private void StartWashing()
    {
        //yield return new WaitForSeconds(startingOffset);
        currentWashEvent.SetupEvent();
        isPlaying = true;
        isTransitioning = false;
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
                currentWashEvent.EndEvent();
                NextEvent();
            }
            else
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

        if (nextEvent == null)
        {
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

    /*public void ChangeEventRandom()
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
    }*/

    /*private void RecordResults(MusicWashEvent washEvent)
    {
        MenuManager.instance.GetMusicResultsMenu().AddWashEventResults(washEvent);
    }*/

    public void SetNonLinearAction(MusicSwitchEvent nextEvent)
    {
        nonLinearCalSwitch.ChooseEvent(nextEvent);
    }

    public void EndGame()
    {
        isPlaying = false;
        isFinished = true;
        float totalScore = MenuManager.instance.GetTotalScore();
        MusicResultsMenu musicResultsMenu = MenuManager.instance.GetMusicResultsMenu();
        musicResultsMenu.AddTotalScore(totalScore);

        // add scores for all the scrub events only
        List<MusicScrubEvent> scrubEvents = GetScrubEvents();
        foreach (MusicScrubEvent scrubEvent in scrubEvents)
        {
            musicResultsMenu.AddWashEventResults(scrubEvent);
        }
        musicResultsMenu.Show();
        MenuManager.instance.HideUIForEndGame();
        AudioManager.instance.PlayOneShot("End Music");
        audioSource.Stop();

        if (SurveyManager.instance != null)
        {
            HandleSurvey();
        }
    }

    private string TimeSpanToString(TimeSpan time)
    {
        string t =  " Minutes: " + Mathf.Round((float)time.TotalMinutes) +
                    " Seconds: " + Mathf.Round((float)time.TotalSeconds);
        return t;
    }

    private string GetTimeTaken()
    {
        TimeSpan timeDifference = DateTime.Now - songStart;
        return TimeSpanToString(timeDifference);
    }

    private void HandleSurvey()
    {
        SurveySongData surveySongData = new SurveySongData();
        surveySongData.songName = songData.name;
        surveySongData.totalPoints = MenuManager.instance.GetTotalScore();
        surveySongData.timeTaken = GetTimeTaken();

        Dictionary<string, float> surveyScrubResults = new Dictionary<string, float>();
        List<MusicScrubEvent> scrubEvents = GetScrubEvents();
        foreach (MusicScrubEvent scrubEvent in scrubEvents)
        {
            surveyScrubResults.Add(scrubEvent.GetEventType().ToString(), scrubEvent.GetScore());
        }

        foreach (NonLinearAnimationSwitch pointEvent in nonScrubPointEvents)
        {
            surveyScrubResults.Add(pointEvent.GetEventType().ToString(), pointEvent.pointsEarned);
        }

        surveySongData.scrubResults = surveyScrubResults;

        SurveyManager.instance.AddSongData(surveySongData);
        SurveyManager.instance.SendDataToServer();
    }

    private List<MusicScrubEvent> GetScrubEvents()
    {
        List<MusicScrubEvent> musicScrubEvents = new List<MusicScrubEvent>();
        foreach(Transform scrubChild in scrubEventsContainer)
        {
            MusicScrubEvent scrubEvent = scrubChild.GetComponent<MusicScrubEvent>();
            if (scrubEvent != null)
            {
                musicScrubEvents.Add(scrubEvent);
            }
        }
        return musicScrubEvents;
    }

    public void OnDestroy()
    {
        eventProvider.Unregister<Beat>(HandleBeat);
    }

    public MusicWashEvent GetCurrentEvent()
    {
        return currentWashEvent;
    }

    // forces the finish of the current event and switches to the next
    public void HardSwitchEvent(MusicWashEvent nextEvent)
    {
        // end prev
        if (GetCurrentEvent() is MusicPlayerEvent)
        {
            MusicPlayerEvent prevPayerEvent = (MusicPlayerEvent)GetCurrentEvent();
            // special setup only for hard switches
            prevPayerEvent.HardSwitchEnd();
        }

        if (nextEvent is MusicPlayerEvent)
        {
            MusicPlayerEvent currPlayerEvent = (MusicPlayerEvent)nextEvent;
            // special setup only for hard switches
            currPlayerEvent.HardSwitchSetup();
        }
        currentWashEvent = nextEvent;
    }

    public void ToggleTransitioning(bool status)
    {
        isTransitioning = status;
    }

    public void ReturnToNonLinearNeutral()
    {
        HardSwitchEvent(_nonLinearCalSwitch);
    }


    public void SetSkin(SkinMapping skin)
    {
        gameSettings.SetChosenSkin(skin);
        Material[] mats = skinnedMesh.materials;
        mats[1] = skin.skinMaterial;
        skinnedMesh.materials = mats;
    }
}
