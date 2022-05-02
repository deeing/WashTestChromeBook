using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeTrialDialog : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleText;

    public void Toggle(bool status)
    {
        gameObject.SetActive(status);
    }

    public void UpdateText(string text)
    {
        titleText.text = text;
    }
}
