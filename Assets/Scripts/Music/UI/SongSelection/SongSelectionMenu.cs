using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectionMenu : SingletonMonoBehaviour<SongSelectionMenu>
{
    [SerializeField]
    private Transform _songSelectionContainer;
    public Transform songSelectionContainer { get => _songSelectionContainer; set => _songSelectionContainer = value; }
    [SerializeField]
    private Button startButton;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        if (!AudioManager.instance.SoundIsPlaying("Main Menu Music"))
        {
            AudioManager.instance.PlaySound("Main Menu Music");
        }
    }

    public void ToggleStartButton(bool status)
    {
        startButton.interactable = status;
    }

    public void StartGame()
    {
        AudioManager.instance.FadeOut("Main Menu Music");
        SongSelection.instance.StartGame();
    }
}
