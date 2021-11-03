using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelection : SingletonMonoBehaviour<SongSelection>
{
    [SerializeField]
    private SongData[] songList;
    [SerializeField]
    private GameObject possibleSongChoicePrefab;

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
            GameObject songChoiceObj = Instantiate(possibleSongChoicePrefab, SongSelectionMenu.instance.songSelectionContainer);
            songChoiceObj.GetComponent<PossibleSongChoice>().Setup(song);
        }
    }

    public void ChooseSong(SongData song)
    {
        selectedSong = song;
        SceneManager.LoadScene("Main");
    }
}
