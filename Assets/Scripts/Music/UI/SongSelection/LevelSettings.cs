using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Dropdown;
using Wash.Utilities;

public class LevelSettings : MonoBehaviour
{
    [SerializeField]
    private Dropdown difficultyDropDown;
    [SerializeField]
    private Toggle touchScreenMode;

    private const string DifficultyString = "PlayerDifficulty";
    private const string TouchScreenMode = "TouchScreen";

    private bool setupDone = false;

    private void Start()
    {
        foreach (LevelDifficulty difficulty in Enum.GetValues(typeof(LevelDifficulty)))
        {
            OptionData option = new OptionData();
            option.text = difficulty.GetDescription();
            difficultyDropDown.options.Add(option);
        }
        difficultyDropDown.value = LoadDifficulty();
        difficultyDropDown.Select();
        difficultyDropDown.RefreshShownValue();

        touchScreenMode.isOn = LoadTouchScreen();
        setupDone = true;
    }

    public void SelectDifficulty(int difficulty)
    {
        SongSelection.instance.SelectDifficulty(difficulty);
        SaveDifficulty(difficulty);

        if (setupDone)
        {
            SongSelectionHintManager.instance.RegisterChoseDifficulty();
        }
    }

    private void SaveDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt(DifficultyString, difficulty);
    }

    private int LoadDifficulty()
    {
        int savedDiff = PlayerPrefs.GetInt(DifficultyString, 0);

        return savedDiff;
    }

    public void SetTouchScreen(bool isOn)
    {
        SongSelection.instance.SetTouchScreen(isOn);
        SaveTouchScreen(isOn);
    }

    private void SaveTouchScreen(bool isOn)
    {
        PlayerPrefs.SetInt(TouchScreenMode, isOn ? 1 : 0);
    }


    private bool LoadTouchScreen()
    {
        int touchMode = PlayerPrefs.GetInt(TouchScreenMode, 0);
        return touchMode == 1 ? true : false;
    }
}
