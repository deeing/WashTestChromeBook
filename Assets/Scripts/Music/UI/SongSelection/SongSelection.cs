using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private bool _nonLinear;
    public bool nonLinear { get => _nonLinear; private set => _nonLinear = value; }

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
        Debug.Log("Setting up song list");
        foreach(SongData song in songList)
        {
            Transform songContainer = SongSelectionMenu.instance.songSelectionContainer;
            GameObject songChoiceObj = Instantiate(possibleSongChoicePrefab, songContainer);
            ToggleColorGroup toggleColorGroup = songContainer.GetComponent<ToggleColorGroup>();
            PossibleSongChoice songChoice = songChoiceObj.GetComponent<PossibleSongChoice>();
            toggleColorGroup.AddToggleColor(songChoice.GetColor());
            songChoice.Setup(song);
        }
    }

    public void ChooseSong(SongData song)
    {
        selectedSong = song;
        SongSelectionMenu.instance.ToggleStartButton(true);
    }

    public void SetNonLinear(bool isOn)
    {
        nonLinear = isOn;
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
