using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Dropdown;

public class DifficultySelection : MonoBehaviour
{
    private Dropdown dropDown;

    private void Awake()
    {
        dropDown = GetComponent<Dropdown>();

        foreach (LevelDifficulty difficulty in Enum.GetValues(typeof(LevelDifficulty)))
        {
            OptionData option = new OptionData();
            option.text = difficulty.ToString();
            dropDown.options.Add(option);
        }
    }
}
