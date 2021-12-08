using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Dropdown;

public class DifficultySelection : MonoBehaviour
{
    private Dropdown dropDown;

    private const string DifficultyString = "PlayerDifficulty";

    private void Awake()
    {
        dropDown = GetComponent<Dropdown>();

        foreach (LevelDifficulty difficulty in Enum.GetValues(typeof(LevelDifficulty)))
        {
            OptionData option = new OptionData();
            option.text = difficulty.ToString();
            dropDown.options.Add(option);
        }
        dropDown.value = LoadDifficulty();
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
}
