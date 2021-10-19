using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckListItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text checkListText;
    [SerializeField]
    private Toggle checkListToggle;

    public void SetText(string text)
    {
        checkListText.text = text;
    }

    public void SetToggle(bool status)
    {
        checkListToggle.isOn = status;
    }
}
