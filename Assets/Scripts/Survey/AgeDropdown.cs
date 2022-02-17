using UnityEngine;
using TMPro;
using static TMPro.TMP_Dropdown;

public class AgeDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void SetAge(int dropDownIndex)
    {
        OptionData chosenOption = dropdown.options[dropDownIndex];
        SurveyManager.instance.SetAge(chosenOption.text);
    }
}
