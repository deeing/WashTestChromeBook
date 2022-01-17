using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Wash.Utilities;

public class StatusDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text statusText;

    public void ShowStatusDisplay(RhythmInputStatus inputStatus)
    {
        // doesn't show if it's a miss?
        if (inputStatus == RhythmInputStatus.Miss)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        statusText.text = inputStatus.GetDescription();
    }

    public void HideStatusDisplay()
    {
        gameObject.SetActive(false);
    }
}
