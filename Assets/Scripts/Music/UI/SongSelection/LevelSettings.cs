using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Dropdown;

public class LevelSettings : MonoBehaviour
{
    [SerializeField]
    private Dropdown difficultyDropDown;
    [SerializeField]
    private Toggle nonLinearToggle;

    private const string DifficultyString = "PlayerDifficulty";
    private const string NonLinearString = "NonLinear";

    private void Start()
    {
        foreach (LevelDifficulty difficulty in Enum.GetValues(typeof(LevelDifficulty)))
        {
            OptionData option = new OptionData();
            option.text = difficulty.ToString();
            difficultyDropDown.options.Add(option);
        }
        difficultyDropDown.value = LoadDifficulty();

        nonLinearToggle.isOn = LoadNonLinear();
    }

    public void SelectDifficulty(int difficulty)
    {
        SongSelection.instance.SelectDifficulty(difficulty);
        SaveDifficulty(difficulty);
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

    public void SetNonLinear(bool isOn)
    {
        SongSelection.instance.SetNonLinear(isOn);
        SaveNonLinear(isOn);
    }

    private void SaveNonLinear(bool isOn)
    {
        PlayerPrefs.SetInt(NonLinearString, isOn ? 1 : 0);
    }

    private bool LoadNonLinear()
    {
        int nonLinear = PlayerPrefs.GetInt(NonLinearString, 0);
        return nonLinear == 1 ? true : false;
    }
}
