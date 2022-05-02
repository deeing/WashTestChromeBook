using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SongSelection : SingletonMonoBehaviour<SongSelection>
{
    [SerializeField]
    private SongData[] songList;
    [SerializeField]
    private GameObject possibleSongChoicePrefab;
    [SerializeField]
    private LevelDifficulty _difficulty;
    public LevelDifficulty difficulty { get => _difficulty; private set => _difficulty = value; }
    [SerializeField]
    private bool _touchScreenMode;
    public bool touchScreenMode { get => _touchScreenMode; private set => _touchScreenMode = value; }
    [SerializeField]
    private int numberSongsLocked = 0;

    public SongData selectedSong { get; private set; }

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            DestroyImmediate(gameObject);
            return;
        }

        //SetupSongList();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SongSelection")
        {
            SetupSongList();
        }
    }

    private void SetupSongList()
    {
        List<PossibleSongChoice> songChoiceList = new List<PossibleSongChoice>();

        Debug.Log("Setting up song list");
        foreach(SongData song in songList)
        {
            Transform songContainer = SongSelectionMenu.instance.songSelectionContainer;
            GameObject songChoiceObj = Instantiate(possibleSongChoicePrefab, songContainer);
            ToggleColorGroup toggleColorGroup = songContainer.GetComponent<ToggleColorGroup>();
            PossibleSongChoice songChoice = songChoiceObj.GetComponent<PossibleSongChoice>();
            songChoiceList.Add(songChoice);
            toggleColorGroup.AddToggleColor(songChoice.GetColor());
            songChoice.Setup(song);
        }

        for(int i=0; i < numberSongsLocked; i++)
        {
            songChoiceList[songChoiceList.Count - 1 - i].SetLocked();
        }
    }

    public void ChooseSong(SongData song)
    {
        selectedSong = song;
        SongSelectionMenu.instance.ToggleStartButton(true);
    }

    public void SetTouchScreen(bool isOn)
    {
        touchScreenMode = isOn;
    }

    public void SelectDifficulty(int difficultyIndex)
    {
        difficulty = (LevelDifficulty)difficultyIndex;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
